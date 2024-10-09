using System.Text.Json.Serialization;
using Sandbox;

namespace Clover;

[Category( "Clover" )]
[Icon( "visibility" )]
[Description( "Handles the visibility of the object based on the world layer." )]
public sealed class WorldLayerObject : Component
{
	
	/// <summary>
	///  The layer of the world this object is in. It's just the index of the world in the WorldManager.
	/// </summary>
	[Property, Sync] public int Layer { get; private set; }
	
	[JsonIgnore] public World World => WorldManager.Instance?.GetWorld( Layer );
	
	public void SetLayer( int layer, bool rebuildVisibility = false )
	{
		Layer = layer;
		
		if ( rebuildVisibility )
		{
			RebuildVisibility();
		}
	}

	public void SetLayerWithTransform( int layer )
	{
		var currentLayer = Layer;
		var currentHeight = WorldPosition.z;
		var layerHeight = WorldManager.WorldOffset;
		
		Layer = layer;
		WorldPosition = new Vector3( WorldPosition.x, WorldPosition.y, currentHeight + ( layerHeight * ( layer - currentLayer ) ) );

	}

	/// <summary>
	///  Visibility is based on render tags on the camera. This method adds or removes the tags based on the layer.
	/// </summary>
	public void RebuildVisibility()
	{
		Tags.Remove( "worldlayer_invisible" );
		Tags.Remove( "worldlayer_visible" );
			
		if ( Layer == WorldManager.Instance.ActiveWorldIndex )
		{
			Tags.Add( "worldlayer_visible" );
		}
		else
		{
			Tags.Add( "worldlayer_invisible" );
		}
	}
}
