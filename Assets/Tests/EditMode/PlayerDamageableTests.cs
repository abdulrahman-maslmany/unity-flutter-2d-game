using NUnit.Framework;

namespace Tests
{
    // اختبارات منطق نظام الضرر وصحة اللاعب
    public class PlayerDamageableCore
    {
        public int Health { get; private set; }
        public int MaxHealth { get; }
        public bool IsAlive => Health > 0;

        public PlayerDamageableCore(int maxHealth = 100)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            Health = System.Math.Max(0, Health - damage);
        }

        public void Revive()
        {
            Health = MaxHealth;
        }
    }

    [TestFixture]
    public class PlayerDamageableTests
    {
        [Test]
        public void TakeDamage_ReducesHealth_Accurately()
        {
            var damageable = new PlayerDamageableCore(100);
            damageable.TakeDamage(35);

            Assert.AreEqual(65, damageable.Health);
            Assert.IsTrue(damageable.IsAlive);
        }

        [Test]
        public void TakeDamage_ExceedingMaxHealth_ClampsAtZeroAndTriggersDeath()
        {
            var damageable = new PlayerDamageableCore(100);
            damageable.TakeDamage(150);

            Assert.AreEqual(0, damageable.Health);
            Assert.IsFalse(damageable.IsAlive);
        }

        [Test]
        public void Revive_RestoresHealthToMax()
        {
            var damageable = new PlayerDamageableCore(100);
            damageable.TakeDamage(100);
            damageable.Revive();

            Assert.AreEqual(100, damageable.Health);
            Assert.IsTrue(damageable.IsAlive);
        }
    }
}
