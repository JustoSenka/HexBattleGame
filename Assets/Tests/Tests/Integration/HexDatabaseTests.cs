using Assets;
using NUnit.Framework;

namespace Tests.Integration
{
    public class HexDatabaseTests
    {
        private DependencyContainer Container;
        private IHexDatabase HexDatabase;
        private ICrossPlayerController CrossPlayerController;

        private Unit m_Unit0, m_Unit1;

        [SetUp]
        public void CreateSomeUnit()
        {
            Container = TestUtility.CreateContainer();
            HexDatabase = Container.Resolve<IHexDatabase>();
            CrossPlayerController = Container.Resolve<ICrossPlayerController>();

            m_Unit0 = TestUtility.CreateUnit(new int2(0, 0), 0);
            m_Unit1 = TestUtility.CreateUnit(new int2(2, 0), 1);

            HexDatabase.AddNewSelectable(m_Unit0);
            HexDatabase.AddNewSelectable(m_Unit1);
        }
         
        [Test]
        public void NewlyAddedUnits_AppearInHexMap()
        {
            var hex0 = HexDatabase.GetHex(new int2(0, 0));
            var hex1 = HexDatabase.GetHex(new int2(1, 0));
            var hex2 = HexDatabase.GetHex(new int2(2, 0));
            Assert.AreEqual(HexType.Unit, hex0.Type, "Hex0 had incorrect type");
            Assert.AreEqual(HexType.Empty, hex1.Type, "Hex1 had incorrect type");
            Assert.AreEqual(HexType.Unit, hex2.Type, "Hex2 had incorrect type");
        }

        [Test]
        public void MovingUnit_WillUpdateHexTypes()
        {
            CrossPlayerController.MoveUnit(m_Unit0, new[] { new int2(1, 0) }, new int2(1, 0));

            var hex0 = HexDatabase.GetHex(new int2(0, 0));
            var hex1 = HexDatabase.GetHex(new int2(1, 0));

            Assert.AreEqual(HexType.Empty, hex0.Type, "Hex0 had incorrect type");
            Assert.AreEqual(HexType.Unit, hex1.Type, "Hex1 had incorrect type");
        }

        [Test]
        public void RemovingUnit_WillUpdateHexTypes()
        {
            HexDatabase.RemoveSelectable(m_Unit0);
            var hex0 = HexDatabase.GetHex(new int2(0, 0));
            Assert.AreEqual(HexType.Empty, hex0.Type, "Hex0 had incorrect type");
        }
    }
}
