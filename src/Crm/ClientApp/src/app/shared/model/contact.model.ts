import { Moment } from 'moment';
import { ISazeman } from 'app/shared/model/sazeman.model';
import { ISemateSazeman } from 'app/shared/model/semate-sazeman.model';

export interface IContact {
  id?: number;
  personCode?: string;
  contactName?: string;
  birthDate?: string;
  description?: any;
  attachFileContentType?: string;
  attachFile?: any;
  sazeman?: ISazeman;
  semateSazeman?: ISemateSazeman;
}

export class Contact implements IContact {
  constructor(
    public id?: number,
    public personCode?: string,
    public contactName?: string,
    public birthDate?: string,
    public description?: any,
    public attachFileContentType?: string,
    public attachFile?: any,
    public sazeman?: ISazeman,
    public semateSazeman?: ISemateSazeman
  ) {}
}
