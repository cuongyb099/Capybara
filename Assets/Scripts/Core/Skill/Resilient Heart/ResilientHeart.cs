using Cysharp.Threading.Tasks;
using Stats.M_Attribute;
using UnityEngine;

namespace Core.Skill
{
    public class ResilientHeart : SkillBase, IDefenseSkill
    {
        protected ResilientHeartData data;
        
        public ResilientHeart(EntityStats owner, ResilientHeartData data) : base(owner)
        {
            this.data = data;
        }

        public override SkillData GetSkillData() => this.data;


        public void OnDamaged(Transform attacker, ref float damageInput)
        {
            var hp = this.owner.GetAttribute(AttributeType.Hp);

            float hpPercentToActive = data.Values[0] / 100;
            
            if(damageInput / hp.MaxValue < hpPercentToActive) return;

            _ = Heal();
        }

        private async UniTaskVoid Heal()
        {
            float timeToHeal = 0.15f;
            await UniTask.WaitForSeconds(timeToHeal);
            var hp = this.owner.GetAttribute(AttributeType.Hp);
            float hpPercentHeal = data.Values[1] / 100;
            float healValue = hp.MaxValue * hpPercentHeal;
            hp.Value += healValue;
            this.owner.TextCombat.CreateHealPopup(healValue, this.owner.transform.position);
        }
    }
}
