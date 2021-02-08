#region Imports
#endregion

#region Program
namespace TechFloor.Object
{
    public class ThreeField<X, Y, Z>
    {
        #region Fields
        private X x_;
        private Y y_;
        private Z z_;
        #endregion

        #region Constructors
        public ThreeField(X first, Y second, Z third)
        {
            x_ = first;
            y_ = second;
            z_ = third;
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

        public Z third
        {
            get => z_;
            set => z_ = value;
        }
        #endregion

        #region Public methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            ThreeField<X, Y, Z> other = obj as ThreeField<X, Y, Z>;
            if (other == null) return false;

            return (((first == null) && (other.first == null))
                    || ((first != null) && first.Equals(other.first))) &&
                    (((second == null) && (other.second == null))
                    || ((second != null) && second.Equals(other.second))) &&
                    (((third == null) && (other.third == null))
                    || ((third != null) && third.Equals(other.third)));
        }

        public override int GetHashCode()
        {
            int hashcode_ = 0;
            if (first != null) hashcode_ += first.GetHashCode();
            if (second != null) hashcode_ += second.GetHashCode();
            if (third != null) hashcode_ += third.GetHashCode();
            return hashcode_;
        }
        #endregion
    }
}
#endregion