using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Attachment
{
    /// <summary>
    /// Обёртка для точки, присоединяемой к фигуре
    /// </summary>
    public class AttachedPoint
    {
        public Point Object { get; set; }
        public float T
        {
            get => _t.T;
            set
            {
                _t.T = value;
            }
        }

        private AttachmentParameter _t;

        public AttachedPoint(Point obj, float t = 0.0f)
        {
            Object = obj;
            _t = new AttachmentParameter() { T = t };
        }
    }
}
