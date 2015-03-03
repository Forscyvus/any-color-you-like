using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {
	public enum Types {
		dirt, lava, concrete, grass, purple
	};

	public Sprite yellTile, purpTile, fireHot, fireCold, fireWall, grassHot, grassCold1, grassCold2, grassWall, metalHot, metalCold, metalWall,
			spookyHot, spookyCold1, spookyCold2, spookyWall, dirto, dirtoWall, oldDirt;

	public SpriteRenderer spriteRenderer;

	public bool isWall;

	public bool isDestructible;

	public Types type;

	public string id;

	public bool isHot;
	public bool wasHot;

	private bool isFirst;

	float HOT_TIMER = 3f;
	float hotTimer;

	// Use this for initialization
	void Start () {
		//id = "0";
		type = Types.dirt;
		spriteRenderer = (SpriteRenderer)gameObject.GetComponent ("SpriteRenderer");
		isHot = false;
		hotTimer = 0;
		isFirst = true;
	}
	
	// Update is called once per frame
	void Update () {
		wasHot = isHot;
		if (hotTimer > 0) {
			hotTimer -= Time.deltaTime;
		} else {
			isHot = false;
			if (wasHot || isFirst){
				isFirst = false;
				if (id.Equals ("2")) {
					spriteRenderer.sprite = Random.Range(0,100) > 75 ? grassCold1 : grassCold2;
				} else if (id.Equals ("4")) {
					spriteRenderer.sprite = Random.Range(0,100) > 75 ? spookyCold2 : spookyCold1;
				}
			}
		}

		if (id.Equals ("0")) {
			if (isWall) {
				spriteRenderer.sprite = dirtoWall;
			} else {
				spriteRenderer.sprite = dirto;
			}
		} else if (id.Equals ("1")) {
			if (isHot) {
				spriteRenderer.sprite = metalHot;
			} else {
				if (isWall) {
					spriteRenderer.sprite = metalWall;
				} else {
					spriteRenderer.sprite = metalCold;
				}
			}
		} else if (id.Equals ("2")) {
			if (isHot) {
				spriteRenderer.sprite = grassHot;
			} else {
				if (isWall) {
					spriteRenderer.sprite = grassWall;
				} else {

				}
			}
		} else if (id.Equals ("3")) {
			if (isHot) {
				spriteRenderer.sprite = fireHot;
			} else {
				if (isWall) {
					spriteRenderer.sprite = fireWall;
				} else {
					spriteRenderer.sprite = fireCold;
				}
			}
		} else if (id.Equals ("4")) {
			if (isHot) {
				spriteRenderer.sprite = spookyHot;
			} else {
				if (isWall) {
					spriteRenderer.sprite = spookyWall;
				} else {
					//spriteRenderer.sprite = Random.Range (0,100) > 75 ? spookyCold2 : spookyCold2;
				}
			}
		} 
	}

	public void setPlayer(string id) {
		isHot = true;
		hotTimer = HOT_TIMER;
		this.id = id;
		/*
		if (id.Equals ( "1")) {
			spriteRenderer.sprite = purpTile;
			type = Types.concrete;
		} else if (id.Equals ( "2")) {
			spriteRenderer.sprite = yellTile;
			type = Types.grass;
		}
*/
	}
}
