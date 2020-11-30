using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crm.Dto {

    public class ContactDto {

        public long Id { get; set; }

        [Required]
        public string PersonCode { get; set; }
        [Required]
        public string ContactName { get; set; }
        public string BirthDate { get; set; }
        public string Description { get; set; }
        public byte[] AttachFile { get; set; }
        //public SazemanDto Sazeman { get; set; }

        public SemateSazemanDto SemateSazeman { get; set; }


        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}
