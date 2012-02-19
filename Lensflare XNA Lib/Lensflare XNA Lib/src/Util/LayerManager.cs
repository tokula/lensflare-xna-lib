using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util {
    public class LayerManager {
        int layerCount;

        public LayerManager(int layerCount) {
            this.layerCount = layerCount;
        }

        public float Depth(int layerIndex) {
            return (float)layerIndex / (float)layerCount;
        }

        public float Depth(int layerIndex, float subdepth) {
            return ((float)layerIndex + subdepth) / (float)layerCount;
        }
    }
}
