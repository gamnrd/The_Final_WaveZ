using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> src;
    [SerializeField] private MeshFilter target;

    [SerializeField] private UnityMeshSimplifier.MeshSimplifier simplify;
    [SerializeField] public float quality;

    [ContextMenu(itemName:"Combine Meshes")]
    private void CombineMeshes()
    {
        var combine = new CombineInstance[src.Count];

        for (int i = 0; i < src.Count; i++)
        {
            combine[i].mesh = src[i].sharedMesh;
            combine[i].transform = src[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        target.mesh = mesh;
    }

    [ContextMenu(itemName: "Simplify Meshes")]
    private void SimplifyMeshes()
    {
        simplify = new UnityMeshSimplifier.MeshSimplifier();
        var mesh = GetComponent<MeshFilter>();
        simplify.Initialize(mesh.sharedMesh);
        simplify.SimplifyMesh(quality);
        mesh.sharedMesh = simplify.ToMesh();
    }
}
