using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int maximum;  
        public int minimum;
        public Count(int min,int max){
            maximum = max;
            minimum = min;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5,9);
    public Count foodCount = new Count(1,5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform BoarderHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList(){
        gridPositions.Clear();
        for(int i=1;i<columns-1;i++){
            for(int j=1;j<rows-1;j++){
                gridPositions.Add(new Vector3(i,j,0f));
            }
        }
    }

    void BoardSetup(){
        Transform boarderHolder = new GameObject("Board").transform;
          for(int i=-1;i<columns+1;i++){
            for(int j=-1;j<rows+1;j++){
                GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
                if(i==-1||i==columns||j==-1||j==rows)
                    toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)];
                
                GameObject instance = Instantiate(toInstantiate, new Vector3(i,j,0f),Quaternion.identity) as GameObject;
                instance.transform.SetParent(boarderHolder);
            } 
        }
    }

    Vector3 RandomPosition(){
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex]; 
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

 void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum+1);
			
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition();

                //Choose a random tile from tileArray and assign it to tileChoice
                GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}

//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			//Creates the outer walls and floor.
			BoardSetup ();
			
			//Reset our list of gridpositions.
			InitialiseList ();
			
			//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
			
			//Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

            //Determine number of enemies based on current level number, based on a logarithmic progression
            int enemyCount = (int)Mathf.Log(level, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
			
			//Instantiate the exit tile in the upper right hand corner of our game board
			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		}
}
