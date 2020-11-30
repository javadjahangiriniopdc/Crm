using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain {
    [Table("sazeman")]
    public class Sazeman {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string SazemanName { get; set; }
        public IList<SemateSazeman> SemateSazemen { get; set; } = new List<SemateSazeman>();

        public IList<Contact> Contacts { get; set; } = new List<Contact>();

        // jhipster-needle-entity-add-field - JHipster will add fields here, do not remove

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var sazeman = obj as Sazeman;
            if (sazeman?.Id == null || sazeman?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, sazeman.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "Sazeman{" +
                    $"ID='{Id}'" +
                    $", SazemanName='{SazemanName}'" +
                    "}";
        }
    }
}
