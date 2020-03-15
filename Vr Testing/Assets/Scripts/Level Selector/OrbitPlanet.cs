using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPlanet : MonoBehaviour
{
    // Maximum distance the object is from what it's orbitting. These extend the specified number of units in both the positive x/z and negative x/z.
    [SerializeField] float xDist;
    [SerializeField] float zDist;

    /*
     * We get a circular orbit if we make these the same.
     * We get an ovular orbit if we make these different.
     * In the future, we can make the shape of the orbit itself rotate by updating these values based on Time.deltaTime.
     * 
     */

    // How much higher / lower the object is from what it's orbitting. Does not effect the vertical angle at all, still is a horizontal orbit.
    [SerializeField] float yOffset;

    // The point it's orbitting.
    [SerializeField] Transform anchor;

    // Speed it rotates around the anchor. Modifier for the timer.
    [SerializeField] float speed;

    // Determines if it rotates clockwise or counter-clockwise.
    [SerializeField] bool rotClockwise;

    // Uses Time.deltaTime to update the position of the object as time passes.
    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        // Updates timer. We use Time.deltaTime as to not update based on framerate.
        // Speed modifies this value, which doesn't make sense because the timer can't tell the time anymore, but we need to update it every frame evenly, so this way is actually more clean.
        // We don't use the timer at all besides multiplying by speed.
        timer += Time.deltaTime * speed;
        Rotate();
    }

    // Does the math to set the position of the object based on the timer.
    void Rotate()
    {
        if(rotClockwise)
        {
            /*
             * Sets the x and z coordinates for the object.
             * Imagine a right triangle with lines in the x and z directions.
             * By doing cosine and sine functions, we can triangulate the distances of the z and x.
             * Since sine and cosine are just slightly offset versions of the same graph, the total amount (distance directly to anchor) is the same at any given moment.
             * 
             * We then multiply by the distance modifer, which sets it out as far as we want.
             * 
             */

            float x = -Mathf.Cos(timer) * xDist;
            float z = Mathf.Sin(timer) * zDist;

            // Creates the new position and sets it.
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + anchor.position;
        } else
        {
            //Same thing as before, but makes the object orbit the other way since one of the trigonemtric formulas isn't negative.
            float x = Mathf.Cos(timer) * xDist;
            float z = Mathf.Sin(timer) * zDist;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + anchor.position;
        }
    }
}
