using System.Collections;
using CoreSystem;
using NUnit.Framework;
using PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using ViewSystem;

namespace PlayModeTestsSystem
{
    public sealed class SceneTests
    {
        [UnitySetUp]
        public IEnumerator LoadScene()
        {
            yield return SceneManager.LoadSceneAsync("RtsTraining", LoadSceneMode.Single);
            yield return null;
            Object.FindObjectOfType<RtsBootstrap>().enabled = false;
        }

        [Test]
        public void MinimalSceneSelectsMovesAndFacesTheDraggedDirection()
        {
            var game = Object.FindObjectOfType<RtsBootstrap>();
            Assert.AreEqual(60, game.Statistics.Total);
            Assert.AreEqual(0, Object.FindObjectsOfType<Canvas>().Length);
            Assert.AreEqual(61, Object.FindObjectsOfType<MeshRenderer>().Length, "Only a plane and 60 capsules.");
            Assert.IsNotNull(GameObject.Find("Ground").GetComponent<MeshCollider>());
            Assert.IsTrue(Camera.main.orthographic);
            Assert.AreEqual(45, Camera.main.transform.eulerAngles.y, 0.001f);
            Assert.AreEqual(35.26439f, Camera.main.transform.eulerAngles.x, 0.001f);
            foreach (var transform in Object.FindObjectsOfType<Transform>(true))
                foreach (char letter in transform.name) Assert.Less((int)letter, 128, transform.name);

            var input = new RtsPointerInput(Camera.main, Object.FindObjectOfType<SelectionFeedback>(), game.Commands);
            Physics.SyncTransforms();
            var first = game.Aspect.Units.FirstSlow().Entity;
            Vector2 point = Camera.main.WorldToScreenPoint(game.Aspect.Positions.Get(first).Value + Vector3.up);
            input.Process(new PointerFrame { Position = point, LeftDown = true, LeftHeld = true });
            input.Process(new PointerFrame { Position = point, LeftUp = true });
            game.Step(0);
            Assert.AreEqual(1, game.Statistics.Selected);

            Vector2 min = new Vector2(float.MaxValue, float.MaxValue), max = new Vector2(float.MinValue, float.MinValue);
            foreach (var e in game.Aspect.Units)
            {
                Vector2 screen = Camera.main.WorldToScreenPoint(game.Aspect.Positions.Get(e).Value + Vector3.up);
                min = Vector2.Min(min, screen); max = Vector2.Max(max, screen);
            }
            input.Process(new PointerFrame { Position = max + Vector2.one * 8, LeftDown = true, LeftHeld = true });
            input.Process(new PointerFrame { Position = min - Vector2.one * 8, LeftHeld = true });
            Assert.IsTrue(GameObject.Find("Selection Rectangle").GetComponent<LineRenderer>().enabled);
            input.Process(new PointerFrame { Position = min - Vector2.one * 8, LeftUp = true });
            game.Step(0);
            Assert.AreEqual(60, game.Statistics.Selected);
            foreach (var e in game.Aspect.Units) Assert.IsTrue(game.Aspect.Views.Get(e).Value.IsHighlighted);

            DragOrder(input, game, new Vector3(9, 0, 6), Vector3.right);
            Assert.AreEqual(60, game.Statistics.Moving);
            for (int i = 0; i < 30; i++) game.Step(1f / 60);
            DragOrder(input, game, new Vector3(-12, 0, -6), Vector3.back);
            for (int i = 0; i < 900; i++) game.Step(1f / 60);
            Assert.AreEqual(0, game.Statistics.Moving);
            foreach (var e in game.Aspect.Units)
            {
                var view = game.Aspect.Views.Get(e).Value;
                Assert.Less(Vector3.Distance(game.Aspect.Positions.Get(e).Value, game.Aspect.Destinations.Get(e).Value), 0.001f);
                Assert.Less(Vector3.Angle(view.transform.forward, Vector3.back), 0.01f);
                Assert.AreEqual(game.Aspect.Positions.Get(e).Value, view.transform.position);
            }
#if UNITY_EDITOR
            CapturePreview();
#endif
            game.Commands.Selection = SelectionKind.Clear; game.Step(0);
            foreach (var e in game.Aspect.Units) Assert.IsFalse(game.Aspect.Views.Get(e).Value.IsHighlighted);
            LogAssert.NoUnexpectedReceived();
        }

        private static void DragOrder(RtsPointerInput input, RtsBootstrap game, Vector3 target, Vector3 direction)
        {
            Vector2 start = Camera.main.WorldToScreenPoint(target);
            Vector2 end = Camera.main.WorldToScreenPoint(target + direction * 3);
            input.Process(new PointerFrame { Position = start, RightDown = true, RightHeld = true });
            Assert.IsFalse(game.Commands.HasMove, "Holding RMB must not issue partial orders.");
            input.Process(new PointerFrame { Position = end, RightHeld = true });
            Assert.IsTrue(GameObject.Find("Facing Preview").GetComponent<LineRenderer>().enabled);
            input.Process(new PointerFrame { Position = end, RightUp = true });
            Assert.IsTrue(game.Commands.HasFacing);
            Assert.Less(Vector3.Distance(target, game.Commands.MovePoint), 0.001f, "Dragging must not move the destination.");
            Assert.Less(Vector3.Angle(direction, game.Commands.FacingDirection), 0.01f);
            game.Step(1f / 60);
        }

        [Test]
        public void PlainClickAndCancelledDragDoNotLeakFacingOrders()
        {
            var game = Object.FindObjectOfType<RtsBootstrap>();
            var input = new RtsPointerInput(Camera.main, Object.FindObjectOfType<SelectionFeedback>(), game.Commands);
            Physics.SyncTransforms();
            Vector2 point = Camera.main.WorldToScreenPoint(Vector3.zero);
            input.Process(new PointerFrame { Position = point, RightDown = true, RightHeld = true });
            input.Cancel(); // Focus lost.
            input.Process(new PointerFrame { Position = point + Vector2.right * 70, RightUp = true });
            Assert.IsFalse(game.Commands.HasMove);
            input.Process(new PointerFrame { Position = point, RightDown = true, RightHeld = true });
            input.Process(new PointerFrame { Position = new Vector2(-10, -10), RightUp = true });
            Assert.IsFalse(game.Commands.HasMove);
            input.Process(new PointerFrame { Position = point, RightDown = true, RightHeld = true });
            input.Process(new PointerFrame { Position = point, RightUp = true });
            Assert.IsTrue(game.Commands.HasMove);
            Assert.IsFalse(game.Commands.HasFacing);
            game.Commands.Clear();
            input.Process(new PointerFrame { Position = point, RightDown = true, RightHeld = true });
            input.Process(new PointerFrame { Position = point, Clear = true });
            input.Process(new PointerFrame { Position = point, RightUp = true });
            Assert.IsFalse(game.Commands.HasMove);
        }

#if UNITY_EDITOR
        private static void CapturePreview()
        {
            var camera = Camera.main;
            var target = new RenderTexture(1600, 900, 24);
            var previous = RenderTexture.active;
            var oldTarget = camera.targetTexture;
            var oldSize = camera.orthographicSize;
            var image = new Texture2D(1600, 900, TextureFormat.RGB24, false);
            try
            {
                camera.targetTexture = target;
                camera.orthographicSize = 17.5f;
                camera.Render();
                RenderTexture.active = target;
                image.ReadPixels(new Rect(0, 0, 1600, 900), 0, 0);
                image.Apply();
                System.IO.Directory.CreateDirectory("Logs");
                System.IO.File.WriteAllBytes("Logs/RtsMinimalPreview.png", image.EncodeToPNG());
            }
            finally
            {
                camera.targetTexture = oldTarget;
                camera.orthographicSize = oldSize;
                RenderTexture.active = previous;
                Object.Destroy(image); Object.Destroy(target);
            }
        }
#endif
    }
}
