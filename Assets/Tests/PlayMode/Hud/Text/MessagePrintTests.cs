using HudNS;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace Hud.Text
{

    public class MessagePrintTests
    {
        MessagePrint messagePrint;
        GameObject infoMessenger;

        [SetUp]
        public void Setup()
        {
            //Arrange
            infoMessenger = GameObject.Instantiate(new GameObject());
            infoMessenger.AddComponent<TextMeshProUGUI>();
            infoMessenger.name = MyConstants.HUDConstants.TEXTSHOWER;
            messagePrint = infoMessenger.AddComponent<MessagePrint>();
        }
        [UnityTest]
        public IEnumerator PrintMessageTest_Expect_PrintedMessageAndChangedOpacity()
        {
            //Act
            messagePrint.PrintMessage("aaa", 1);
            //Assert
            Assert.AreEqual("aaa", infoMessenger.GetComponent<TextMeshProUGUI>().text);
            Assert.IsTrue(infoMessenger.GetComponent<TextMeshProUGUI>().alpha == 1f);
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(infoMessenger.GetComponent<TextMeshProUGUI>().alpha < 1f);
        }
    }
}
