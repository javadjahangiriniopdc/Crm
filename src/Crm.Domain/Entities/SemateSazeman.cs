using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain {
    [Table("semate_sazeman")]
    public class SemateSazeman {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string SemateSazemanName { get; set; }
        public IList<Contact> Contacts { get; set; } = new List<Contact>();

        public Sazeman Sazeman { get; set; }

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var semateSazeman = obj as SemateSazeman;
            if (semateSazeman?.Id == null || semateSazeman?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, semateSazeman.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "SemateSazeman{" +
                    $"ID='{Id}'" +
                    $", SemateSazemanName='{SemateSazemanName}'" +
                    "}";
        }
    }
}
