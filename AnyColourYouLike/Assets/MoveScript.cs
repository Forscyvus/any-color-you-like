using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
    public Transform player;
	public Vector2 moveVector;
	private float MAX_SPEED = 50f;
	public enum Facing {
		right, left, up, down
	};
	public Facing direction;

	public GameObject builder;

	private TileBuilder builderScript;

	private TileController tileController;

	public string id;

	public Transform respawnLocation;

	public float respawnTimer;
	float RESPAWN_TIMER = 5f;

	private int SHOT_LENGTH = 4;
	public float shotCooldown;
	private float SHOT_COOLDOWN = 1f;

	private float shotDelay;
	private float SHOT_DELAY = .25f;

	public float percentage;

	public Sprite up2, down2, left2, right2, downCast, leftCast, rightCast, upCast;

	private SpriteRenderer rend;

	private AudioSource src;

	// Use this for initialization
	void Start () {
		moveVector = Vector2.zero;
		direction = Facing.down;
		builderScript = builder.GetComponent ("TileBuilder") as TileBuilder; //ok
		shotCooldown = 0;
		tileController = null;
		player.position = respawnLocation.position;
		respawnTimer = 0;
		percentage = 7.8125f;
		shotDelay = SHOT_DELAY;
		rend = (SpriteRenderer)gameObject.GetComponent ("SpriteRenderer");
		src = (AudioSource)gameObject.GetComponent ("AudioSource");
	}
	
	// Update is called once per frame
	void Update () {

		/* Dis is direction stuff */
		float horizontal = Input.GetAxis("Horizontal" + id);
		float vertical = Input.GetAxis("Vertical" + id);
		if (horizontal > 0f && horizontal > Mathf.Abs (vertical)) {
			direction = Facing.right;
			rend.sprite = right2;
		}
		if (horizontal < 0f && Mathf.Abs(horizontal) > Mathf.Abs (vertical)) {
			direction = Facing.left;		
			rend.sprite = left2;
		}
		if (vertical > 0 && vertical > Mathf.Abs (horizontal)) {
			direction = Facing.up;		
			rend.sprite = up2;
		}
		if (vertical < 0 && Mathf.Abs(vertical) > Mathf.Abs (horizontal)) {
			direction = Facing.down;		
			rend.sprite = down2;
		}

		/* Finds the stood upon tile */
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tile");
		ArrayList wallz = new ArrayList ();
		GameObject closest = tiles [0];
		GameObject secondClosest = tiles [0];
		float closestdist = Vector3.Distance (player.position, closest.transform.position);
		float secondClosestdist = Vector3.Distance (player.position, secondClosest.transform.position);
		foreach (GameObject tile in tiles) {
			tileController = (TileController)(tile.GetComponent ("TileController"));
			if (tileController.isWall) {
				wallz.Add (tile);
			}
			if (Vector3.Distance(player.position, tile.transform.position) < closestdist) {
				secondClosest = closest;
				secondClosestdist = closestdist;
				closest = tile;
				closestdist = Vector3.Distance(player.position, tile.transform.position);
			} else if (Vector3.Distance (player.position, tile.transform.position) < secondClosestdist) {
				secondClosest = tile;
				secondClosestdist = Vector3.Distance (player.position, tile.transform.position);
			}
		}
		Transform[,] tilez = builderScript.tilez;
		int closestx = (((int)closest.transform.position.x-8) / 16) + 12;
		int closesty = (((int)closest.transform.position.y-8) / 16) + 12;
		int nextClosestx = (((int)secondClosest.transform.position.x-8) / 16) + 12;
		int nextClosesty = (((int)secondClosest.transform.position.y-8) / 16) + 12;


		/* DEATH HANDLING */
		tileController = (TileController)(tilez[closestx,closesty].gameObject.GetComponent ("TileController"));
		if (respawnTimer > 0) {
			respawnTimer -= Time.deltaTime;
			if (respawnTimer < .3333f) {
				player.gameObject.renderer.enabled = false;
			} else if (respawnTimer < .6666f) {
				player.gameObject.renderer.enabled = true;
			} else if (respawnTimer < 1f) {
				player.gameObject.renderer.enabled = false;
			} else if (respawnTimer < 1.3333f) {
				player.gameObject.renderer.enabled = true;
			} else if (respawnTimer < 1.6666f) {
				player.gameObject.renderer.enabled = false;
			} else if (respawnTimer < 2f) {
				player.gameObject.renderer.enabled = true;
			}
			return;
		} else {
			player.gameObject.renderer.enabled = true;
		}
		if (respawnTimer <= 0 && !tileController.id.Equals (id) && tileController.isHot) {
			respawnTimer = RESPAWN_TIMER;
			player.position = respawnLocation.position;
			direction = Facing.down;
			player.gameObject.renderer.enabled = false;
			return;
		}


		/* Dis fires */
		if (Input.GetAxis ("Fire" + id) != 0 && shotCooldown <= 0 && shotDelay == SHOT_DELAY) {
			shotDelay -= Time.deltaTime;
		}
		if (shotDelay < SHOT_DELAY && shotDelay >= 0) {
			shotDelay -= Time.deltaTime;
			if (direction == Facing.up) {
				rend.sprite = upCast;
			} else if (direction == Facing.down) {
				rend.sprite = downCast;
			} else if (direction == Facing.right) {
				rend.sprite = rightCast;
			} else if (direction == Facing.left) {
				rend.sprite = leftCast;
			}
			return;
		}
		if (shotDelay < 0) {
			src.Play ();
			shotDelay = SHOT_DELAY;
			shotCooldown = SHOT_COOLDOWN;
			if (direction == Facing.up) {
				for (int i = 0; i < SHOT_LENGTH; i++) {
					if (closesty + i < 24) {
						tileController = (TileController)(tilez[closestx,closesty+i].gameObject.GetComponent ("TileController"));
						tileController.setPlayer(id);
					}
				}
			} else if (direction == Facing.down) {
				for (int i = 0; i < SHOT_LENGTH; i++) {
					if (closesty - i >= 0) {
						tileController = (TileController)(tilez[closestx,closesty-i].gameObject.GetComponent ("TileController"));
						tileController.setPlayer(id);
					}
				}
			} else if (direction == Facing.left) {
				for (int i = 0; i < SHOT_LENGTH; i++) {
					if (closestx - i >= 0) {
						tileController = (TileController)(tilez[closestx-i,closesty].gameObject.GetComponent ("TileController"));
						tileController.setPlayer(id);
					}
				}
			} else if (direction == Facing.right) {
				for (int i = 0; i < SHOT_LENGTH; i++) {
					if (closestx + i < 24) {
						tileController = (TileController)(tilez[closestx+i,closesty].gameObject.GetComponent ("TileController"));
						tileController.setPlayer(id);	
					}
				}
			}
			percentage = builderScript.checkFill(closestx, closesty, direction, SHOT_LENGTH);

		}
		//shot cooldown update
		if (shotCooldown > 0) {
			shotCooldown -= Time.deltaTime;
		}

		/* DIS MOVE */
		if (player.position.x > 185 && horizontal > 0) {
			horizontal = 0;
		}
		if (player.position.x < -185 && horizontal < 0) {
			horizontal = 0;
		}
		if (player.position.y > 185 && vertical > 0) {
			vertical = 0;
		}
		if (player.position.y < -185 && vertical < 0) {
			vertical = 0;
		}
		foreach (GameObject tile in wallz) {
			if (player.position.y > tile.transform.position.y - 8 && player.position.y < tile.transform.position.y + 16) {
				if (player.position.x >= tile.transform.position.x - 16 && player.position.x <= tile.transform.position.x - 8 && horizontal > 0) {
					horizontal = 0;
					player.transform.position = new Vector3(tile.transform.position.x - 16, player.position.y, 0);
				} else if (player.position.x <= tile.transform.position.x + 16 && player.position.x >= tile.transform.position.x + 8 && horizontal < 0) {
					horizontal = 0;
					player.transform.position = new Vector3(tile.transform.position.x + 16, player.position.y, 0);
				}
			}
			if (player.position.x > tile.transform.position.x - 16 && player.position.x < tile.transform.position.x + 16) {
				if (player.position.y >= tile.transform.position.y - 8 && player.position.y <= tile.transform.position.y - 4 && vertical > 0) {
					vertical = 0;
					player.transform.position = new Vector3(player.position.x, tile.transform.position.y - 8, 0);
				} else if (player.position.y <= tile.transform.position.y + 16 && player.position.y >= tile.transform.position.y + 8 && vertical < 0) {
					vertical = 0;
					player.transform.position = new Vector3(player.position.x, tile.transform.position.y + 16, 0);
				}
			}
		}
		/*
		tileController = (TileController)(tilez[closestx,closesty].gameObject.GetComponent ("TileController"));
		if (tileController.isWall) {
			print(closestx);
			print(nextClosestx);
			if (closestx == nextClosestx) {
				bool ternaryShit = (player.position.y - secondClosest.transform.position.y) < 0;
				player.Translate (new Vector3(0, (player.position.y - secondClosest.transform.position.y - (8 * (ternaryShit ? -1 : 1))), 0));
			} else if (closesty == nextClosesty) {
				bool ternaryShit = (player.position.x - secondClosest.transform.position.x) < 0;
				player.Translate (new Vector3(0, (player.position.x - secondClosest.transform.position.x - (8 * (ternaryShit ? -1 : 1))), 0));
			}
		}
		*/
		/*
		tileController = (TileController)(tilez [nextClosestx, nextClosesty].gameObject.GetComponent ("TileController"));
		if (tileController.isWall) {
			if (nextClosestx > closestx && horizontal > 0) {
				horizontal = 0;
			} else if (nextClosestx < closestx && horizontal < 0) {
				horizontal = 0;
			}
			if (nextClosesty > closesty && vertical > 0) {
				vertical = 0;
			} else if (nextClosesty < closesty && vertical < 0) {
				vertical = 0;
			}
		}
		*/
		moveVector = new Vector2(horizontal, vertical);
		moveVector.Normalize ();
		moveVector *= MAX_SPEED;
		player.Translate( moveVector * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D trigger) {
		print ("yousuck");
	}
}
















