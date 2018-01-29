using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
namespace StarForce
{
    public class ChooseMonstorEvent : GameEventArgs
    {
        /// <summary>
        /// 加载场景成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ChooseMonstorEvent).GetHashCode();
        public DRMonster mData;
        public ChooseMonstorEvent(DRMonster mons)
        {
            mData = mons;
        }

        /// <summary>
        /// 获取加载场景成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        /// <summary>

        /// <summary>
        /// 清理加载场景成功事件。
        /// </summary>
        public override void Clear()
        {
        }

    }

}
