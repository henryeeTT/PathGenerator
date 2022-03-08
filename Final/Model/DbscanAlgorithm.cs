using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Vector3 = OpenTK.Vector3;

namespace Final.Model {
    public class DbscanAlgorithm<T> where T : TriMesh {
        private readonly Func<T, T, double> _metricFunc;

        public DbscanAlgorithm (Func<T, T, double> metricFunc) {
            _metricFunc = metricFunc;
        }

        public DbscanAlgorithm () {
        }

        public void ComputeClusterDbscan (T[] allPoints, double epsilon, int minPts, out HashSet<T[]> clusters) {
            DbscanPoint<T>[] allPointsDbscan = allPoints.Select(x => new DbscanPoint<T>(x)).ToArray();
            int clusterId = 0;

            for (int i = 0; i < allPointsDbscan.Length; i++) {
                DbscanPoint<T> p = allPointsDbscan[i];
                if (p.IsVisited)
                    continue;
                p.IsVisited = true;

                RegionQuery(allPointsDbscan, p.ClusterPoint, epsilon, out DbscanPoint<T>[] neighborPts);
                if (neighborPts.Length < minPts)
                    p.ClusterId = (int)ClusterIds.Noise;
                else {
                    clusterId++;
                    ExpandCluster(allPointsDbscan, p, neighborPts, clusterId, epsilon, minPts);
                }
            }

            clusters = new HashSet<T[]>(
                allPointsDbscan
                    .Where(x => x.ClusterId > 0)
                    .GroupBy(x => x.ClusterId)
                    .Select(x => x.Select(y => y.ClusterPoint).ToArray())
                );
        }

        private void ExpandCluster (DbscanPoint<T>[] allPoints, DbscanPoint<T> point, DbscanPoint<T>[] neighborPts, int clusterId, double epsilon, int minPts) {
            point.ClusterId = clusterId;
            for (int i = 0; i < neighborPts.Length; i++) {
                DbscanPoint<T> pn = neighborPts[i];
                if (!pn.IsVisited) {
                    pn.IsVisited = true;
                    DbscanPoint<T>[] neighborPts2;
                    RegionQuery(allPoints, pn.ClusterPoint, epsilon, out neighborPts2);
                    if (neighborPts2.Length >= minPts) {
                        neighborPts = neighborPts.Union(neighborPts2).ToArray();
                    }
                }
                if (pn.ClusterId == (int)ClusterIds.Unclassified)
                    pn.ClusterId = clusterId;
            }
        }

        private void RegionQuery (DbscanPoint<T>[] allPoints, T point, double epsilon, out DbscanPoint<T>[] neighborPts) {
            neighborPts = (from p in allPoints.AsParallel()
                           where Math.Abs(p.ClusterPoint.norm.X - point.norm.X) < 0.1
                           where Math.Abs(p.ClusterPoint.norm.Y - point.norm.Y) < 0.1
                           where Math.Abs(p.ClusterPoint.norm.Z - point.norm.Z) < 0.1
                           where _metricFunc(point, p.ClusterPoint) <= epsilon
                           select p).ToArray();
        }

        public enum ClusterIds {
            Unclassified = 0,
            Noise = -1
        }

        public class DbscanPoint<T> {
            public bool IsVisited;
            public T ClusterPoint;
            public int ClusterId;

            public DbscanPoint (T x) {
                ClusterPoint = x;
                IsVisited = false;
                ClusterId = (int)ClusterIds.Unclassified;
            }

        }

    }
}
