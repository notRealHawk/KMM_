using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCycling : MonoBehaviour
{
    //public Feature playerObject;
	public Playershirt playershirt;
	public Playerhat playerhat;
	public Playerpant playerpant;
    public List<GameObject> Hats = new List<GameObject>();
    public List<Material> HatColor = new List<Material>();
    public List<GameObject> Shirts = new List<GameObject>();
	public List<GameObject> Pants = new List<GameObject>();
    int currentHatIndex = 0;
    int currentShirtIndex = 0;
	int currentPantIndex = 0;
    int currentcolor = 0;
    
    public void nextHat(){
    	currentHatIndex++;
    	if(currentHatIndex >= Hats.Count){
    		currentHatIndex = 0;
    	}
	    playerhat.Hat.GetComponent<MeshFilter>().mesh = Hats[currentHatIndex].GetComponent<MeshFilter>().sharedMesh;
    }
    public void prevHat(){
    	currentHatIndex--;
    	if(currentHatIndex <= 0){
    		currentHatIndex = 1;
    	}
	    playerhat.Hat.GetComponent<MeshFilter>().mesh = Hats[currentHatIndex].GetComponent<MeshFilter>().sharedMesh;
    }
    public void nextColor(){
    	currentcolor++;
    	if(currentcolor >= HatColor.Count){
    		currentcolor = 0;
    	}
	    playerhat.Hat.GetComponent<Renderer>().material = HatColor[currentcolor];
    }
    public void prevColor(){
    	currentcolor--;
    	if(currentcolor <= 0){
    		currentcolor = 1;
    	}
	    playerhat.Hat.GetComponent<Renderer>().material = HatColor[currentcolor];
    }
    public void nextShirt(){
    	currentShirtIndex++;
    	if(currentShirtIndex >= Shirts.Count){
    		currentShirtIndex = 0;
    	}
	    playershirt.Shirt.GetComponent<MeshFilter>().mesh = Shirts[currentShirtIndex].GetComponent<MeshFilter>().sharedMesh;
	    playershirt.Shirt.GetComponent<Transform>().transform.localScale = Shirts[currentShirtIndex].GetComponent<Transform>().transform.localScale;
	    playershirt.ShirtMaterial = Shirts[currentShirtIndex].GetComponent<Renderer>().sharedMaterial;
	    playershirt.Shirt.GetComponent<Renderer>().material = playershirt.ShirtMaterial;
    }
    public void prevShirt(){
    	currentShirtIndex--;
    	if(currentShirtIndex <= 0){
    		currentShirtIndex = 1;
    	}
	    playershirt.Shirt.GetComponent<MeshFilter>().mesh = Shirts[currentShirtIndex].GetComponent<MeshFilter>().sharedMesh;
	    playershirt.Shirt.GetComponent<Transform>().transform.localScale = Shirts[currentShirtIndex].GetComponent<Transform>().transform.localScale;
	    playershirt.ShirtMaterial = Shirts[currentShirtIndex].GetComponent<Renderer>().sharedMaterial;
	    playershirt.Shirt.GetComponent<Renderer>().material = playershirt.ShirtMaterial;
    }
	public void nextPant(){
		currentPantIndex++;
		if(currentPantIndex >= Pants.Count){
			currentPantIndex = 0;
		}
		playerpant.Pant.GetComponent<MeshFilter>().mesh = Pants[currentPantIndex].GetComponent<MeshFilter>().sharedMesh;
		playerpant.Pant.GetComponent<Transform>().transform.localScale = Pants[currentPantIndex].GetComponent<Transform>().transform.localScale;
		playerpant.PantMaterial = Pants[currentPantIndex].GetComponent<Renderer>().sharedMaterial;
		playerpant.Pant.GetComponent<Renderer>().material = playerpant.PantMaterial;
	}
	public void prevPant(){
		currentPantIndex--;
		if(currentPantIndex <= 0){
			currentPantIndex = 1;
		}
		playerpant.Pant.GetComponent<MeshFilter>().mesh = Pants[currentPantIndex].GetComponent<MeshFilter>().sharedMesh;
		playerpant.Pant.GetComponent<Transform>().transform.localScale = Pants[currentPantIndex].GetComponent<Transform>().transform.localScale;
		playerpant.PantMaterial = Pants[currentPantIndex].GetComponent<Renderer>().sharedMaterial;
		playerpant.Pant.GetComponent<Renderer>().material = playerpant.PantMaterial;
	}
}
