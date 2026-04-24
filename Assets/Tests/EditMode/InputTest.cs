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

        // nagyon fontos: ne fusson semmi Unity logika
        player.enabled = false;
    }

    [Test]
    public void SetMoveDirection_SetsCorrectValue()
    {
        Vector2 dir = new Vector2(1, 0);

        player.SetMoveDirection(dir);

        var field = typeof(Player_Movement)
            .GetField("moveInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Vector2 value = (Vector2)field.GetValue(player);

        Assert.AreEqual(dir, value);
    }

    [Test]
    public void RequestJump_WhenGrounded_SetsJumpRequest()
    {
        // Arrange
        typeof(Player_Movement)
            .GetField("isGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(player, true);

        // Act
        player.RequestJump();

        // Assert
        var jumpField = typeof(Player_Movement)
            .GetField("jumpRequest", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        bool jumpRequested = (bool)jumpField.GetValue(player);

        Assert.IsTrue(jumpRequested);
    }

    [Test]
    public void ResetJumpPadBoost_SetsFalse_AfterIteration()
    {
        player.isJumpPadBoost = true;

        var enumerator = player.ResetJumpPadBoost();

        // végigpörgetjük a coroutine-t
        while (enumerator.MoveNext()) { }

        Assert.IsFalse(player.isJumpPadBoost);
    }



}
