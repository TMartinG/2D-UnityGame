using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class PlayerCameraTest
{
    private Player_Camera cam;
    private GameObject camObj;
    private GameObject targetObj;

    [SetUp]
    public void Setup()
    {
        camObj = new GameObject();
        cam = camObj.AddComponent<Player_Camera>();

        targetObj = new GameObject();
        targetObj.transform.position = Vector3.zero;

        typeof(Player_Camera)
            .GetField("target", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(cam, targetObj.transform);
    }

    [Test]
    public void SetBoundsBack_ResetsBounds()
    {
        cam.minX = 10;
        cam.maxX = 20;
        cam.minY = 5;
        cam.maxY = 15;

        cam.SetBoundsBack();

        Assert.AreEqual(-9999, cam.minX);
        Assert.AreEqual(9999, cam.maxX);
        Assert.AreEqual(-9999, cam.minY);
        Assert.AreEqual(9999, cam.maxY);
    }

    [Test]
    public void SetBounds_AppliesZoneValues()
    {
        var zoneObj = new GameObject();
        var zone = zoneObj.AddComponent<Camera_Zone>();

        zone.isMinX = true;
        zone.minX = -5;

        zone.isMaxX = true;
        zone.maxX = 10;

        cam.SetBounds(zone);

        Assert.AreEqual(-5, cam.minX);
        Assert.AreEqual(10, cam.maxX);
    }

    [Test]
    public void LateUpdate_DoesNothing_WhenTargetNull()
    {
        typeof(Player_Camera)
            .GetField("target", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(cam, null);

        Assert.DoesNotThrow(() =>
        {
            cam.LateUpdate();
        });
    }

    [Test]
    public void Camera_ClampsPositionCorrectly()
    {
        cam.minX = 0;
        cam.maxX = 5;
        cam.minY = 0;
        cam.maxY = 5;

        cam.offset = Vector3.zero;

        targetObj.transform.position = new Vector3(100, 100, 0);

        cam.LateUpdate();

        Vector3 pos = cam.transform.position;

        Assert.LessOrEqual(pos.x, 5);
        Assert.LessOrEqual(pos.y, 5);
    }
}
