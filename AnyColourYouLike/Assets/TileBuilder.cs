using UnityEngine;
using System.Collections;

public class TileBuilder : MonoBehaviour {
	public Transform tile;
	public Sprite teil;
	public Transform[,] tilez;

	private TileController tileController;
	// Use this for initialization
	void Start () {
		tilez = new Transform[24,24];
		for (int x = -12; x < 12; x++) {
			for(int y = -12; y < 12; y++) {
				Transform clone;
				clone = (Transform)Instantiate(tile, new Vector2((x+.5f)*teil.rect.width,(y+.5f)*teil.rect.height), Quaternion.identity);
				tileController = (TileController)(clone.gameObject.GetComponent ("TileController"));
				tileController.setPlayer ("0");
				if (y - x > 14) {
					tileController.setPlayer ("1");
				} else if (x - y > 14) {
					tileController.setPlayer ("2");
				} else if (x + y >= 14) {
					tileController.setPlayer ("3");
				} else if (x + y < -15) {
					tileController.setPlayer ("4");
				}

				if (x == 0 || x == -1) {
					if ( (y > -9 && y < -4) || (y < 8 && y > 3) ) {
						tileController.isWall = true;
					}
				}
				if (y == 0 || y == -1) {
					if ( (x > -9 && x < -4) || (x < 8 && x > 3) ) {
						tileController.isWall = true;
					}
				}
				if (x == -9 || x == 8) {
					if (y < 3 && y > -4) {
						tileController.isWall=true;
					}
				}
				if (y == -9 || y == 8) {
					if (x < 3 && x > -4) {
						tileController.isWall = true;
					}
				}

				clone.gameObject.tag = "tile";
				tilez[x+12,y+12] = clone;

			}
		}
	}

	public float checkFill(int x, int y, MoveScript.Facing direction, int shotLength) {
		tileController = (TileController)(tilez[x,y].gameObject.GetComponent ("TileController"));
		string player = tileController.id;


		ArrayList origins = new ArrayList ();
		int xprime = 0;
		int yprime = 0;
		if (direction == MoveScript.Facing.left) {
						xprime = -1;
				}
		if (direction == MoveScript.Facing.right) {
						xprime = 1;
				}
		if (direction == MoveScript.Facing.up) {
						yprime = 1;
				}
		if (direction == MoveScript.Facing.down) {
						yprime = -1;
				}
		for (int i = 0; i < shotLength; i++) {
			if (x+1+(xprime*i) >= 0 && x+1+(xprime*i) < 24 && y+(yprime*i) >= 0 && y+(yprime*i) < 24) {
				tileController = (TileController)(tilez[x+1+(xprime*i),y+(yprime*i)].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals (player)) {
					Node n = new Node(x+1+(xprime*i),y+(yprime*i));
					if (!origins.Contains (n)) {
						origins.Add (n);
					}
				}
			}
			if (x-1+(xprime*i) >= 0 && x-1+(xprime*i) < 24 && y+(yprime*i) >= 0 && y+(yprime*i) < 24) {
				tileController = (TileController)(tilez[x-1+(xprime*i),y+(yprime*i)].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals (player)) {
					Node n = new Node(x-1+(xprime*i),y+(yprime*i));
					if (!origins.Contains (n)) {
						origins.Add (n);
					}
				}
			}
			if (x+(xprime*i) >= 0 && x+(xprime*i) < 24 && y+1+(yprime*i) >= 0 && y+1+(yprime*i) < 24) {
				tileController = (TileController)(tilez[x+(xprime*i),y+1+(yprime*i)].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals (player)) {
					Node n = new Node(x+(xprime*i),y+1+(yprime*i));
					if (!origins.Contains (n)) {
						origins.Add (n);
					}
				}
			}
			if (x+(xprime*i) >= 0 && x+(xprime*i) < 24 && y-1+(yprime*i) >= 0 && y-1+(yprime*i) < 24) {
				tileController = (TileController)(tilez[x+(xprime*i),y-1+(yprime*i)].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals (player)) {
					Node n = new Node(x+(xprime*i),y-1+(yprime*i));
					if (!origins.Contains (n)) {
						origins.Add (n);
					}
				}
			}
		}
		//origins now set up


		for (int i = 0; i < origins.Count; i++) {
			Stack s = new Stack();
			bool fart = true;
			ArrayList visited = new ArrayList();
			Node n = (Node)origins[i];
			Node compensation = new Node(-1,-1);
			origins[i] = compensation;
			visited.Add (n);

			if (n.x+1 < 24) {
			tileController = (TileController)(tilez[n.x+1, n.y].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals ( player)) {
					Node child = new Node(n.x+1, n.y);
					if (!visited.Contains(child)) {
						if (origins.Contains (child)) {
							origins.Remove (child);
						}
						if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
							fart = false;
						}
						s.Push (child);
					}
				}
			}
			if (n.x-1 >= 0) {
				tileController = (TileController)(tilez[n.x-1, n.y].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals ( player)) {
					Node child = new Node(n.x-1, n.y);
					if (!visited.Contains(child)) {
						if (origins.Contains (child)) {
							origins.Remove (child);
						}
						if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
							fart = false;
						}
						s.Push (child);
					}
				}
			}
			if (n.y+1 < 24) {
				tileController = (TileController)(tilez[n.x, n.y+1].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals ( player)) {
					Node child = new Node(n.x, n.y+1);
					if (!visited.Contains(child)) {
						if (origins.Contains (child)) {
							origins.Remove (child);
						}
						if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
							fart = false;
						}
						s.Push (child);
					}
				}
			}
			if (n.y-1 >= 0) {
				tileController = (TileController)(tilez[n.x, n.y-1].gameObject.GetComponent ("TileController"));
				if (!tileController.id.Equals ( player)) {
					Node child = new Node(n.x, n.y-1);
					if (!visited.Contains(child)) {
						if (origins.Contains (child)) {
							origins.Remove (child);
						}
						if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
							fart = false;
						}
						s.Push (child);
					}
				}
			}

			while (s.Count > 0) {
				n = (Node)s.Pop ();
				visited.Add (n);

				if (n.x+1 < 24) {
					tileController = (TileController)(tilez[n.x+1, n.y].gameObject.GetComponent ("TileController"));
					if (!tileController.id.Equals ( player)) {
						Node child = new Node(n.x+1, n.y);
						if (!visited.Contains(child)) {
							if (origins.Contains (child)) {
								origins.Remove (child);
							}
							if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
								fart = false;
							}
							s.Push (child);
						}
					}
				}
				if (n.x-1 >= 0) {
					tileController = (TileController)(tilez[n.x-1, n.y].gameObject.GetComponent ("TileController"));
					if (!tileController.id.Equals ( player)) {
						Node child = new Node(n.x-1, n.y);
						if (!visited.Contains(child)) {
							if (origins.Contains (child)) {
								origins.Remove (child);
							}
							if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
								fart = false;
							}
							s.Push (child);
						}
					}
				}
				if (n.y+1 < 24) {
					tileController = (TileController)(tilez[n.x, n.y+1].gameObject.GetComponent ("TileController"));
					if (!tileController.id.Equals ( player)) {
						Node child = new Node(n.x, n.y+1);
						if (!visited.Contains(child)) {
							if (origins.Contains (child)) {
								origins.Remove (child);
							}
							if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
								fart = false;
							}
							s.Push (child);
						}
					}
				}
				if (n.y-1 >= 0) {
					tileController = (TileController)(tilez[n.x, n.y-1].gameObject.GetComponent ("TileController"));
					if (!tileController.id.Equals ( player)) {
						Node child = new Node(n.x, n.y-1);
						if (!visited.Contains(child)) {
							if (origins.Contains (child)) {
								origins.Remove (child);
							}
							if (child.x == 0 || child.x == 23 || child.y == 0 || child.y == 23) {
								fart = false;
							}
							s.Push (child);
						}
					}
				}
			}
			if (fart) {
				for (int j = 0; j < visited.Count; j++) {
					Node t = (Node)visited[j];
					tileController = (TileController)(tilez[t.x, t.y].gameObject.GetComponent ("TileController"));
					tileController.setPlayer (player);
				}
			}
		}
		//tiles now filled in

		//checking for winner
		float playerCount = 0;
		for (int i = 0 ; i < 24 ; i++) {
			for (int j = 0; j < 24; j++) {
				tileController = (TileController)(tilez[i, j].gameObject.GetComponent ("TileController"));
				if (tileController.id.Equals (player)) {
					playerCount++;
				}
			}
		}
		if (playerCount / (24*24) > .5f) {
			if (player.Equals ("1")) {
				Application.LoadLevel("indwins");
			} else if (player.Equals ("2")) {
				Application.LoadLevel("envwins");
			} else if (player.Equals ("3")) {
				Application.LoadLevel("arswins");
			} else if (player.Equals ("4")) {
				Application.LoadLevel ("cultwins");
			}
		}
		return 100*playerCount / (24*24);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private class Node {
		public int x;
		public int y;

		public Node(int x, int y) {
							this.x = x;
							this.y = y;
						}

		public override bool Equals(object obj) {
						Node n = (Node)obj;
						return (x == n.x && y == n.y);
		}
	}
}