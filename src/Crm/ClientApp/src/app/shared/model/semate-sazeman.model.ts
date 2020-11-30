import { IContact } from 'app/shared/model/contact.model';
import { ISazeman } from 'app/shared/model/sazeman.model';

export interface ISemateSazeman {
  id?: number;
  semateSazemanName?: string;
  contacts?: IContact[];
  sazeman?: ISazeman;
}

export class SemateSazeman implements ISemateSazeman {
  constructor(public id?: number, public semateSazemanName?: string, public contacts?: IContact[], public sazeman?: ISazeman) {}
}
