using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest
{
    private GameObject playerObj;
    private Player_Movement player;
    private Rigidbody2D rb;

    [SetUp]
    public void Setup()
    {
         playerObj = new GameObject();
        rb = playerObj.AddComponent<Rigidbody2D>();
        player = playerObj.AddComponent<Player_Movement>();

        playerObj.AddComponent<BoxCollider2D>();

        player.GetType().GetField("characterBody", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject("body").transform);

        player.GetType().GetField("characterHead", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject("head").transform);

        player.GetType().GetField("characterFeet1", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject("feet1").transform);

        player.GetType().GetField("characterFeet2", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject("feet2").transform);

        player.GetType().GetField("characterEye", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject("eye").transform);

        player.GetType().GetField("characterLight", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, new GameObject("light").transform);

        var camObj = new GameObject("MainCamera");
        camObj.tag = "MainCamera";
        camObj.AddComponent<Camera>();

        var crosshairObj = new GameObject();
        var crosshair = crosshairObj.AddComponent<Crosshair>();

        crosshair.enabled = false;

        player.GetType().GetField("playerCrosshair", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(player, crosshair);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(playerObj);
    }

    [Test]
    public void SetMoveDirectionSetsInput()
    {
        Vector2 dir = new Vector2(1, 0);
        player.SetMoveDirection(dir);

        var field = typeof(Player_Movement)
            .GetField("moveInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Vector2 value = (Vector2)field.GetValue(player);

        Assert.AreEqual(dir, value);
    }

    [Test]
    public void UIOpenAndCloseChangesState()
    {
        GameObject panel = new GameObject();
        panel.SetActive(false);

        var openMethod = typeof(Player_Movement)
            .GetMethod("OpenPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var closeMethod = typeof(Player_Movement)
            .GetMethod("ClosePanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        openMethod.Invoke(player, new object[] { panel });

        Assert.IsTrue(Player_Movement.isUIOpen);

        closeMethod.Invoke(player, new object[] { panel });

        Assert.IsFalse(Player_Movement.isUIOpen);
    }
}
