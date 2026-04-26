using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InputTest
{
    private Player_Movement player;

    [SetUp]
    public void Setup()
    {
        var obj = new GameObject();
        player = obj.AddComponent<Player_Movement>();

        player.enabled = false;
    }

    [Test]
    public void SetMoveDirectionValues()
    {
        Vector2 dir = new Vector2(1, 0);

        player.SetMoveDirection(dir);

        var field = typeof(Player_Movement)
            .GetField("moveInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Vector2 value = (Vector2)field.GetValue(player);

        Assert.AreEqual(dir, value);
    }

    [Test]
    public void RequestJump()
    {
        typeof(Player_Movement)
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(player, true);

        player.RequestJump();

        var jumpField = typeof(Player_Movement)
            .GetField("jumpRequest", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        bool jumpRequested = (bool)jumpField.GetValue(player);

        Assert.IsTrue(jumpRequested);
    }

    [Test]
    public void ResetJumpPadBoost()
    {
        player.isJumpPadBoost = true;

        var enumerator = player.ResetJumpPadBoost();

        // végigpörgetjük a coroutine-t
        while (enumerator.MoveNext()) { }

        Assert.IsFalse(player.isJumpPadBoost);
    }



}
