using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain {
    [Table("contact")]
    public class Contact {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string PersonCode { get; set; }
        [Required]
        public string ContactName { get; set; }
        public string BirthDate { get; set; }
        public string Description { get; set; }
        public byte[] AttachFile { get; set; }
        //public Sazeman Sazeman { get; set; }

        public SemateSazeman SemateSazeman { get; set; }

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var contact = obj as Contact;
            if (contact?.Id == null || contact?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, contact.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "Contact{" +
                    $"ID='{Id}'" +
                    $", PersonCode='{PersonCode}'" +
                    $", ContactName='{ContactName}'" +
                    $", BirthDate='{BirthDate}'" +
                    $", Description='{Description}'" +
                    $", AttachFile='{AttachFile}'" +
                    "}";
        }
    }
}
