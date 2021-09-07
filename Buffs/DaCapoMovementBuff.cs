using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DaCapo.Buffs
{
    public class DaCapoMovement1Buff : BaseDaCapoMovementBuff
    {
        public override int Color => ItemRarityID.Cyan;
        public override string BuffName => "<1st Movement: Adagio>";
        public override string BuffNameCN => "<第一乐章——Adagio>";
        public override string BuffDescription =>
@"'The conductor begins to direct the apocalypse.'
You are immune to projectile damage, and have 75% DR against melee damage.
Generate a small damage aura, the immune frames of enemies in the aura will be temporarily removed.
The upper limit is 50%.
";

        public override string BuffDescriptionCD =>
@"“乐团开始演奏末日的乐章。”
免疫弹幕伤害，对近战伤害有75%减伤
产生一个小的伤害圈，圈中的敌怪会被逐渐暂时移除无敌帧，上限为50%。
";
    }

    public class DaCapoMovement2Buff : BaseDaCapoMovementBuff
    {
        public override int Color => ItemRarityID.White;
        public override string BuffName => "<Movements 2: Sostenuto>";
        public override string BuffNameCN => "<第二乐章——Sostenuto>";
        public override string BuffDescription =>
@"'People start to forget everything and show fervent adoration toward the symphony.
The emotion soon turns into internal insanity, driving them to attack anyone near them.'
You are immune to melee damage, and have 75% DR against projectile damage.
The damage aura expands, and Enemies in the aura will be applied with Fervent Adoration debuff.
With this debuff, enemies will be damaged by other enemies.
";
        public override string BuffDescriptionCD =>
@"“人们逐渐丧失记忆，陶醉在乐团的演出之中。
悠扬的乐声会唤醒心灵最深处的疯狂，促使人们对眼前的一切活物发起攻击。”
免疫近战伤害，对远程伤害有75%减伤
伤害圈扩大，圈中的敌怪会被施加狂热崇拜debuff，此时敌怪会受到其他敌怪的接触和弹幕伤害。
";
    }

    public class DaCapoMovement3Buff : BaseDaCapoMovementBuff
    {
        public override int Color => ItemRarityID.Purple;
        public override string BuffName => "<Movements 3: Accelerando>";
        public override string BuffNameCN => "<第三乐章——Accelerando>";
        public override string BuffDescription =>
@"'The orchestra gives impetus to the music, bringing every individual to his demise.'
You are immune to projectile damage, and have 75% DR against melee damage.
The damage aura expands further, and enemies who have Fervent Adoration debuff will take 5x damage from other enemies.
Reduce their movement speed by 50%.
";
        public override string BuffDescriptionCD =>
@"“当乐章行进到这一步时，乐团加快了节奏，带领每一个人走向毁灭。”
免疫弹幕伤害，对近战伤害有75%减伤
伤害圈进一步扩大，陷入狂热崇拜的敌怪受到的友军伤害增至5倍，同时降低50%移动速度。
";
    }

    public class DaCapoMovement4Buff : BaseDaCapoMovementBuff
    {
        public override int Color => ItemRarityID.Red;
        public override string BuffName => "<Movements 4: Stringendo>";
        public override string BuffNameCN => "<第四乐章——Stringendo>";
        public override string BuffDescription =>
@"'When all the performers have gathered, the music that no one can hear but everyone can listen to begins.'
You are immune to melee damage, and have 75% DR against projectile damage.
The damage aura reaches its maximum size, and the effect of removing the immune frames increases.
The upper limit increases to 75%.
";
        public override string BuffDescriptionCD =>
@"“当所有的演奏家聚集在一起时，乐团将会演奏除它们以外无人能够听闻的天籁之音。”
免疫近战伤害，对远程伤害有75%减伤
伤害圈达到最大，圈中的敌怪无敌帧移除效果加剧，上限增至75%。
";
    }

    public class DaCapoFinalBuff : BaseDaCapoMovementBuff
    {
        public override int Color => ItemRarityID.Expert;
        public override string BuffName => "<Finale: Con Fuoco, Ma Non Troppo>";
        public override string BuffNameCN => "<终章——Con Fuoco, Ma Non Troppo>";
        public override string BuffDescription =>
@"'The music shall perforate your entire being.'
You are immune to all damage.
At this time, the damage aura will spread rapidly, dealing massive damage to enemies in the screen.
If the enemy has Fervent Adoration debuff before, the damage will double and its immune frame is temporarily reduced by 95%.
";
        public override string BuffDescriptionCD =>
@"“这首乐章将会穿透你的灵魂。”
免疫所有伤害
此时伤害圈会迅速扩散，对全屏敌怪造成大量伤害
若敌怪在之前陷入狂热崇拜，则伤害翻倍，且无敌帧暂时降低95%。
";
    }
}
