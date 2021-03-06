﻿using Physics2D;
using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D.Object;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFDemo.Graphic;

namespace WPFDemo.CircleDemo
{
    public class CircleDemo : PhysicsGraphic, IDrawable
    {
        #region 私有字段
        /// <summary>
        /// 中心点
        /// </summary>
        private readonly Particle _centerObj;
        /// <summary>
        /// 物体列表
        /// </summary>
        private readonly List<Particle> _objList = new List<Particle>();
        #endregion

        #region 构造方法
        public CircleDemo(Image image)
            : base(image)
        {
            // 初始化中心点
            _centerObj = PhysicsWorld.CreateFixedParticle
            (
                new Vector2D
                (
                    250.ToSimUnits(),
                    200.ToSimUnits()
                )
            );
            // 注册绘制对象
            DrawQueue.Add(this);
        }
        #endregion

        #region 实现PhysicsGraphic
        protected override void UpdatePhysics(double duration)
        {
            foreach (var item in _objList)
            {
                var v = _centerObj.Position - item.Position;
                item.AddForce(v.Normalize() * 30);
            }
            PhysicsWorld.Update(duration);
        }
        #endregion

        #region 响应鼠标事件
        public void Fire()
        {
            if (!Start)
            {
                // 全局增加一个小阻尼
                PhysicsWorld.CreateGlobalZone(new ParticleDrag(0.01, 0.02));
                Start = true;
            }
            var item = PhysicsWorld.CreateParticle
            (
                new Vector2D(200.ToSimUnits(), 200.ToSimUnits()),
                new Vector2D(0, 5),
                1
            );
            _objList.Add(item);
        }
        #endregion

        #region 实现IDrawable
        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.FillEllipseCentered
            (
                _centerObj.Position.X.ToDisplayUnits(),
                _centerObj.Position.Y.ToDisplayUnits(), 6, 6, Colors.Red
            );
            for (int i = _objList.Count - 1; i >= 0; i--)
            {
                int x = _objList[i].Position.X.ToDisplayUnits();
                int y = _objList[i].Position.Y.ToDisplayUnits();

                bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
            }
        }
        #endregion
    }
}