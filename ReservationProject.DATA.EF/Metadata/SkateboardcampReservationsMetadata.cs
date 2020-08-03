using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ReservationProject.DATA.EF//.Metadata
{
    #region LocationMetadata
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {

    }

    public class LocationMetadata
    {
        //public int LocationId { get; set; }

        [Required(ErrorMessage = "** Location is Required **")]
        [StringLength(50, ErrorMessage = "** Maximum 50 Characters **")]
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [StringLength(30, ErrorMessage = "** Maximum 30 Characters **")]
        [Display(Name = "Instructor")]
        public string InstructorName { get; set; }

        [Required(ErrorMessage = "** Address is Required **")]
        [StringLength(100, ErrorMessage = "** Maximum 100 Characters **")]
        public string Address { get; set; }

        [Required(ErrorMessage = "** City is Required **")]
        [StringLength(100, ErrorMessage = "** Maximum 100 Characters **")]
        public string City { get; set; }

        [Required(ErrorMessage = "** State is Required **")]
        [StringLength(2, ErrorMessage = "** Maximum 2 Characters **")]
        public string State { get; set; }

        [Required(ErrorMessage = "** Zip Code is Required **")]
        [StringLength(5, ErrorMessage = "** Maximum 5 Characters **")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "** Reservation Limit is Required **")]
        [Display(Name = "Participant Limit")]
        public byte ReservationLimit { get; set; }
    }
    #endregion

    #region Owner Asset Metadata
    [MetadataType(typeof(OwnerAssetMetadata))]
    public partial class OwnerAsset
    {

    }

    public class OwnerAssetMetadata
    {
        //public int OwnerAssetId { get; set; }
        [Required(ErrorMessage = "** Child's Name is Required **")]
        [StringLength(50, ErrorMessage = "** Maximum 50 Characters **")]
        [Display(Name = "Child's Name")]
        public string ChildName { get; set; }

        public string OwnerId { get; set; }

        [StringLength(50, ErrorMessage = "** Maximum 50 Characters **")]
        [Display(Name = "Child's Photo")]
        public string ChildPhoto { get; set; }

        [StringLength(300, ErrorMessage = "** Maximum 300 Characters **")]
        [UIHint("MultilineText")]
        public string SpecialNotes { get; set; }
        public bool IsActive { get; set; }

        [DisplayFormat(DataFormatString ="{0:d}")]
        public System.DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "** Date of Birth is Required **")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime ChildDOB { get; set; }

        [StringLength(50, ErrorMessage = "** Maximum 50 Characters **")]
        public string SkillLevel { get; set; }
    }
    #endregion

    #region ReservationMetadata
    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    {

    }

    public class ReservationMetadata
    {
        //public int ReservationId { get; set; }
        [Required(ErrorMessage ="** Owner Asset ID is Required **")]
        public int OwnerAssetId { get; set; }
        [Required(ErrorMessage = "** Location ID is Required **")]
        public int LocationId { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Date")]
        public System.DateTime ReservationDate { get; set; }
    }
    #endregion

    #region UserDetailMetadata
    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        public string Parent
        {
            get { return FirstName + " " + LastName;}
        }
    }

    public class UserDetailMetadata
    {
        //public string UserId { get; set; }
        [Required(ErrorMessage = "** First Name is Required **")]
        [StringLength(50, ErrorMessage = "** Maximum 50 Characters **")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "** Last Name is Required **")]
        [StringLength(50, ErrorMessage = "** Maximum 50 Characters **")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
    #endregion
}
