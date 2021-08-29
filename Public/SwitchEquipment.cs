using UnityEngine;
using System.Collections;

public class SwitchEquipment : MonoBehaviour 
{
	public GameObject shorty;
	private GameObject shortyOnceWorn;
	private bool isWorn = false;

    void Start()
    {
        AddEquipment();
    }
    
	private void AddEquipment()
	{
		isWorn = true;
		
		SkinnedMeshRenderer[] BonedObjects = shorty.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObject( smr ); 
		
		shorty.SetActiveRecursively( false );
	}
	
	private void RemoveEquipment()
	{
		isWorn = false;		
		
		Destroy( shortyOnceWorn );
		
		shorty.SetActiveRecursively( true );
	}
	
	private void ProcessBonedObject( SkinnedMeshRenderer ThisRenderer )
	{		
		shortyOnceWorn = new GameObject( ThisRenderer.gameObject.name );
		
	    shortyOnceWorn.transform.parent = transform;
	
	    SkinnedMeshRenderer NewRenderer = shortyOnceWorn.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
	
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform ); 
	
	    NewRenderer.bones = MyBones;
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh;
	    NewRenderer.materials = ThisRenderer.materials;
	}
	
	private Transform FindChildByName( string ThisName, Transform ThisGObj )	
	{	
	    Transform ReturnObj;
	
	    if( ThisGObj.name == ThisName )	
	        return ThisGObj.transform;
	
	    foreach( Transform child in ThisGObj )	
	    {	
		    ReturnObj = FindChildByName( ThisName, child );
	
	        if( ReturnObj != null )	
	            return ReturnObj;	
	    }
	
	    return null;	
	}	
}
