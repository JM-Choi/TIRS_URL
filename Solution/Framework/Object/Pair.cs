#region Imports
#endregion

#region Program
namespace TechFloor.Object
{
    public class Pair<X, Y>
    {
        #region Fields
        private X x_;
        private Y y_;
        #endregion

        #region Constructors
        public Pair(X first, Y second)
        {
            x_ = first;
            y_ = second;
        }
        #endregion

        #region Porperties
        public X first
        {
            get => x_;
            set => x_ = value;
        }

        public Y second
        {
            get => y_;
            set => y_ = value;
        }
        #endregion

        #region Public methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            Pair<X, Y> other = obj as Pair<X, Y>;
            if (other == null) return false;

            return (((first == null) && (other.first == null))
                    || ((first != null) && first.Equals(other.first))) &&
                    (((second == null) && (other.second == null))
                    || ((second != null) && second.Equals(other.second)));
        }

        public override int GetHashCode()
        {
            int hashcode_ = 0;
            if (first != null) hashcode_ += first.GetHashCode();
            if (second != null) hashcode_ += second.GetHashCode();
            return hashcode_;
        }
        #endregion
    }
}
#endregion