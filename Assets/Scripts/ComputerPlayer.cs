using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public class ComputerPlayer : MonoBehaviour {

	// TODO
	public class ArrayEqualityComparer : IEqualityComparer<int[]> {
		public bool Equals(int[] x, int[] y) {
			if (x.Length != y.Length) {
				return false;
			}
			for (int i = 0; i < x.Length; i++) {
				if (x[i] != y[i]) {
					return false;
				}
			}
			return true;
		}
		
		public int GetHashCode(int[] obj) {
			int hash = obj.Length;
			for(int i = 0; i < obj.Length; ++i)
			{
				 hash = unchecked(hash * 314159 + obj[i]);
			}
			return hash;
		}
	}

    public GameControllerSIM GC;

	static int playerNone = -1;
	static int player1 = 0;
	static int player2 = 1;
	static int minimaxDepth = 9;
	static int minMoveTimeMS = 200;
	private static Dictionary<string, int[]> lineMap;

	static ComputerPlayer() {
		lineMap = new Dictionary<string, int[]>();
		lineMap.Add("LineBlank (1)", new int[] { 0, 1 });
		lineMap.Add("LineBlank (10)", new int[] { 0, 2 });
		lineMap.Add("LineBlank (7)", new int[] { 0, 3 });
		lineMap.Add("LineBlank (9)", new int[] { 0, 4 });
		lineMap.Add("LineBlank", new int[] { 0, 5 });
		lineMap.Add("LineBlank (2)", new int[] { 1, 2 });
		lineMap.Add("LineBlank (14)", new int[] { 1, 3 });
		lineMap.Add("LineBlank (8)", new int[] { 1, 4 });
		lineMap.Add("LineBlank (13)", new int[] { 1, 5 });
		lineMap.Add("LineBlank (3)", new int[] { 2, 3 });
		lineMap.Add("LineBlank (11)", new int[] { 2, 4 });
		lineMap.Add("LineBlank (6)", new int[] { 2, 5 });
		lineMap.Add("LineBlank (4)", new int[] { 3, 4 });
		lineMap.Add("LineBlank (12)", new int[] { 3, 5 });
		lineMap.Add("LineBlank (5)", new int[] { 4, 5 });
	}

	public void getTurn() {
		Stopwatch timer = new Stopwatch();
		timer.Start();
		// build game state
		GameObject[] lineObjects = getAllLines();
		Dictionary<int[], int> gameState = new Dictionary<int[], int>(new ArrayEqualityComparer());
		foreach (GameObject lineObject in lineObjects) {
			LineColorer line = lineObject.GetComponent<LineColorer>();
			gameState.Add(lineMap[lineObject.name], line.whichPlayer);
		}
		if (getAvailableMoves(gameState).Count == 0) {
			return;
		}
		// get best move
		int playerCode = getCurrentPlayer();
		int[] bestMove = getMoveMinimaxPrune(gameState, playerCode);
		// get the line object of best move
		GameObject bestLine = null;
		foreach (GameObject line in getAllLines()) {
			int[] code = lineMap[line.name];
			if (code.SequenceEqual(bestMove)) {
				bestLine = line;
				break;
			}
		}
		// do move
		timer.Stop();
		int runtime = (int) timer.ElapsedMilliseconds;
		if (runtime < minMoveTimeMS) {
			System.Threading.Thread.Sleep(minMoveTimeMS - runtime);
		}
        bestLine.GetComponent<LineColorer>().doTurn();
    }

	private int[] getMoveMinimaxPrune(Dictionary<int[], int> gameState, int player) {
		List<int[]> freeMoves = getAvailableMoves(gameState);
		int bestRate = -200;
		int[] bestMove = null;
		int alpha = -200;
		int beta = 200;
		foreach (int[] move in freeMoves) {
			Dictionary<int[], int> gameStateNew = new Dictionary<int[], int>(gameState, new ArrayEqualityComparer());
			gameStateNew[move] = player;
			int rate = minimaxPrune(gameStateNew, minimaxDepth - 1, player, false, alpha, beta);
			if (rate > bestRate) {
				bestRate = rate;
				bestMove = move;
			}
			if (bestRate > alpha) {
				alpha = bestRate;
			}
			if (alpha >= beta) {
				break;
			}
		}
		return bestMove;
	}

	private int minimaxPrune(Dictionary<int[], int> gameState, int depth, int player, bool maximize, int alpha, int beta) {
		int loser = getLoser(gameState);
		if (loser != playerNone) {
			if (loser == player) {
				return -100;
			}
			else {
				return 100;
			}
		}
		if (depth == 0) {
			return getRate(gameState, player);
		}
		List<int[]> freeMoves = getAvailableMoves(gameState);
		int bestRate = 0;
		if (maximize) {
			// maximize
			bestRate = -200;
			foreach (int[] move in freeMoves) {
				Dictionary<int[], int> gameStateNew = new Dictionary<int[], int>(gameState, new ArrayEqualityComparer());
				gameStateNew[move] = player;
				int moveRate = minimaxPrune(gameStateNew, depth - 1, player, false, alpha, beta) + 1; // "+ 1" to prefer losing later
				if (moveRate > bestRate) {
					bestRate = moveRate;
				}
				if (bestRate > alpha) {
					alpha = bestRate;
				}
				if (alpha >= beta) {
					break;
				}
			}
		} else {
			// minimize
			bestRate = 200;
			foreach (int[] move in freeMoves) {
				Dictionary<int[], int> gameStateNew = new Dictionary<int[], int>(gameState, new ArrayEqualityComparer());
				gameStateNew[move] = otherPlayer(player);
				int moveRate = minimaxPrune(gameStateNew, depth - 1, player, true, alpha, beta) - 1; // "- 1" to prefer losing later
				if (moveRate < bestRate) {
					bestRate = moveRate;
				}
				if (bestRate < beta) {
					beta = bestRate;
				}
				if (alpha >= beta) {
					break;
				}
			}
		}
		return bestRate;
	}

	private int getRate(Dictionary<int[], int> gameState, int player) {
		int loser = getLoser(gameState);
		if (loser == playerNone) {
			return 0;
		} else if (loser == player) {
			return -100;
		} else {
			return 100;
		}
	}

	private int otherPlayer(int player) {
		if (player == player1) {
			return player2;
		} else if (player == player2) {
			return player1;
		} else {
			throw new SystemException();
		}
	}

	private List<int[]> getAvailableMoves(Dictionary<int[], int> gameState) {
		List<int[]> freeMoves = new List<int[]>();
		foreach (KeyValuePair<int[], int> entry in gameState) {
			if (entry.Value == playerNone) {
				freeMoves.Add(entry.Key);
			}
		}
		return freeMoves;
	}

	private int getLoser(Dictionary<int[], int> gameState) {
		for (int i1 = 0; i1 < 4; i1++) {
			for (int i2 = i1 + 1; i2 < 5; i2++) {
				for (int i3 = i2 + 1; i3 < 6; i3++) {
					if (gameState[new int[] { i1, i2 }] == gameState[new int[] { i2, i3 }] &&
						gameState[new int[] { i2, i3 }] == gameState[new int[] { i1, i3 }]) {
						return gameState[new int[] { i1, i2 }];
					}
				}
			}
		}
		return playerNone;
	}
	
    private GameObject[] getAllLines() {
        return GC.allLinePrefabs;
    }

    private int getCurrentPlayer() {
        return GC.WhoseTurn();
    }

    private GameObject getTurnRandom() {
		GameObject[] freeMoves = GC.freeLinePrefabs;
		return freeMoves[UnityEngine.Random.Range(0, freeMoves.Length)];
    }
}
