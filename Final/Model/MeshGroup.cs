using System.Linq;

namespace PathGenerator.Model {
    public class MeshGroup {
        public TriMesh[] meshes;
        public byte[] color = new byte[3];
        public int ID;
        public bool groupA = false;
        public bool groupB = false;
        
        public MeshGroup (TriMesh[] meshes, byte[] color, int ID) {
            this.meshes = meshes.Select(x => new TriMesh(x)).ToArray();
            color.CopyTo(this.color, 0);
            this.ID = ID;
        }
    }
}
