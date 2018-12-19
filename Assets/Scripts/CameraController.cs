using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject _Player;
    Collider _PlayerCol;
    public GameObject _RaycastPoint;
    //public GameObject _Camera;
    public float _CamMovingSpeed;

    RaycastHit hit;

    Collider _TempCol;
    Material _TempMaterial;
    Color _TempColor;

    public Material _TransparentMaterial;

    public float _TransparencySlider;
    // Use this for initialization
    void Start ()
    {
        _RaycastPoint = _Player;
        _PlayerCol = _Player.GetComponent<SphereCollider> ();
        _TransparencySlider = 1;
    }

    // Update is called once per frame

    private void Update ()
    {
        CameraRay ();
    }

    void LateUpdate ()
    {
        transform.position = Vector3.Lerp (transform.position, _Player.transform.position + new Vector3 (-10, 10, -10), _CamMovingSpeed * Time.deltaTime);
        transform.LookAt (_Player.transform);
    }

    void CameraRay ()
    {
        if (Physics.Raycast (transform.position, _RaycastPoint.transform.position - transform.position, out hit))
        {
            //Check if ray doesnt hit the player
            if (hit.collider != _PlayerCol)
            {
                //if both temp variables are null check the gameobject that ray hits and save its collider and material on temp variables
                if (_TempCol == null && _TempMaterial == null)
                {
                    _TempCol = hit.collider;
                    _TempMaterial = hit.collider.GetComponent<MeshRenderer> ().material;
                    print ("temp values saved");
                }
                //if temp values are different than the gameobject ray hits
                //return variables from temp and add new values to it
                else if (_TempCol != hit.collider && _TempMaterial != hit.collider.GetComponent<MeshRenderer> ().material)
                {
                    print ("new temp values saved");
                    _TempCol.GetComponent<MeshRenderer> ().material = _TempMaterial;

                    _TempCol = hit.collider;
                    _TempMaterial = hit.collider.GetComponent<MeshRenderer> ().material;
                }
                //if ray hits player return tempmaterial
                else if (hit.collider == _PlayerCol)
                {
                    _TempCol.GetComponent<MeshRenderer> ().material = _TempMaterial;
                    _TempCol = null;
                    _TempMaterial = null;
                }
                else
                {
                    print ("temp values already saved");
                    return;
                }
                hit.collider.GetComponent<MeshRenderer> ().material = _TransparentMaterial;
                StartCoroutine (ChangeTransparency ());
            }
            else
            {
                if (!_TempCol.GetComponent<MeshRenderer> ()) { return; }
                _TempCol.GetComponent<MeshRenderer> ().material = _TempMaterial;
            }
            //hit.collider.GetComponent<MeshRenderer>().material = _TransparentMaterial;
        }
        else //If ray hits player
        {
            print ("Playerhit");
            Debug.DrawRay (transform.position, _RaycastPoint.transform.position - transform.position, Color.red);
        }
    }

    IEnumerator ChangeTransparency ()
    {

        while (_TransparencySlider >= 0.1)
        {
            _TransparentMaterial.color = new Color (255, 255, 255, 0); //_TransparencySlider);
            _TransparencySlider -= 1f;
            yield return new WaitForSeconds (0.1f);
        }

        _TransparencySlider = 1;
    }
}