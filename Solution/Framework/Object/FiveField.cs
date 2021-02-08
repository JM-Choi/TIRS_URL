#region Imports
#endregion

#region Program
namespace TechFloor.Object
{
    public class FiveField<X, Y, Z, R, T>
    {
        #region Fields
        private X x_;
        private Y y_;
        private Z z_;
        private R r_;
        private T t_;
        #endregion

        #region Constructors
        public FiveField(X first, Y second, Z third, R fourth, T fifth)
        {
            x_ = first;
            y_ = second;
            z_ = third;
            r_ = fourth;
            t_ = fifth;
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

        public R fourth
        {
            get => r_;
            set => r_ = value;
        }

        public T fifth
        {
            get => t_;
            set => t_ = value;
        }
        #endregion

        #region Public methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            FiveField<X, Y, Z, R, T> other = obj as FiveField<X, Y, Z, R, T>;
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
                    || ((fifth != null) && third.Equals(other.fifth)));
        }

        public override int GetHashCode()
        {
            int hashcode_ = 0;
            if (first != null) hashcode_ += first.GetHashCode();
            if (second != null) hashcode_ += second.GetHashCode();
            if (third != null) hashcode_ += third.GetHashCode();
            if (fourth != null) hashcode_ += fourth.GetHashCode();
            if (fifth != null) hashcode_ += fifth.GetHashCode();
            return hashcode_;
        }
        #endregion
    }
}
#endregion