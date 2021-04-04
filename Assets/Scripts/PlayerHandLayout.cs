using System;
using System.Collections.Generic;
using UnityEngine;

namespace RrTestTask
{
    public sealed class PlayerHandLayout
    {
        public IReadOnlyList<Tuple<Vector3, Quaternion>> Calculate(float radius, float cardAngularSize, int count)
        {
            var layouts = new List<Tuple<Vector3, Quaternion>>();
            float startAngle = cardAngularSize * (count - 1) / 2f;
            float endAngle = -cardAngularSize * (count - 1) / 2f;

            for (float angle = startAngle; angle >= endAngle; angle -= cardAngularSize)
            {
                float x = -Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
                float y = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                var position = new Vector3(x, y);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                layouts.Add(new Tuple<Vector3, Quaternion>(position, rotation));
            }

            return layouts;
        }
    }
}