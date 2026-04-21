namespace HN.Code.Weapon.Core
{
    public interface IWeaponLogic
    {
        public void Initialize(WeaponDataSO weaponData);
        public void Enter();
        public void Update();
        public void Shoot();
        public void Exit();
    }
}