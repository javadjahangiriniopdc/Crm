import { ISemateSazeman } from 'app/shared/model/semate-sazeman.model';
import { IContact } from 'app/shared/model/contact.model';

export interface ISazeman {
  id?: number;
  sazemanName?: string;
  semateSazemen?: ISemateSazeman[];
  contacts?: IContact[];
}

export class Sazeman implements ISazeman {
  constructor(public id?: number, public sazemanName?: string, public semateSazemen?: ISemateSazeman[], public contacts?: IContact[]) {}
}
