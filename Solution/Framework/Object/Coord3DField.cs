#region Imports
#endregion

#region Program
namespace TechFloor.Object
{
    public class Coord3DField<X, Y, Z, Rx, Ry, Rz>
    {
        #region Fields
        private X x_;
        private Y y_;
        private Z z_;
        private Rx rx_;
        private Ry ry_;
        private Rz rz_;
        #endregion

        #region Constructors
        public Coord3DField(X first, Y second, Z third, Rx fourth, Ry fifth, Rz sixth)
        {
            x_ = first;
            y_ = second;
            z_ = third;
            rx_ = fourth;
            ry_ = fifth;
            rz_ = sixth;
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

        public Rx fourth
        {
            get => rx_;
            set => rx_ = value;
        }

        public Ry fifth
        {
            get => ry_;
            set => ry_ = value;
        }

        public Rz sixth
        {
            get => rz_;
            set => rz_ = value;
        }
        #endregion

        #region Public methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            Coord3DField<X, Y, Z, Rx, Ry, Rz> other = obj as Coord3DField<X, Y, Z, Rx, Ry, Rz>;
            if (other == null) return false;

            return (((first == null) && (other.first == null))
                    || ((first != null) && first.Equals(other.first))) &&
                    (((second == null) && (other.second == null))
                    || ((second != null) && second.Equals(other.second))) &&
                    (((third == null) && (other.third == null))
                    || ((third != null) && third.Equals(other.third))) &&
                    (((fourth == null) && (other.fourth == null))
                    || ((fourth != null) && third.Equals(other.fourth))) &&
                    (((fifth == null) && (other.fifth == null))
                    || ((fifth != null) && third.Equals(other.fifth))) &&
                    (((sixth == null) && (other.sixth == null))
                    || ((sixth != null) && third.Equals(other.sixth)));
        }

        public override int GetHashCode()
        {
            int hashcode_ = 0;
            if (first != null) hashcode_ += first.GetHashCode();
            if (second != null) hashcode_ += second.GetHashCode();
            if (third != null) hashcode_ += third.GetHashCode();
            if (fourth != null) hashcode_ += fourth.GetHashCode();
            if (fifth != null) hashcode_ += fifth.GetHashCode();
            if (sixth != null) hashcode_ += sixth.GetHashCode();
            return hashcode_;
        }
        #endregion
    }
}
#endregion