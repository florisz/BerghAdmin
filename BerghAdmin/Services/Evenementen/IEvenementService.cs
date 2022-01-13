﻿using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IEvenementService
{
    ErrorCodeEnum SaveEvenement(Evenement evenement);
    Evenement? GetById(int id);
    Evenement? GetByName(string name);
    IEnumerable<T>? GetAllEvenementen<T>();
    ErrorCodeEnum AddDeelnemer(Evenement evenement, Persoon persoon);
    ErrorCodeEnum AddDeelnemer(Evenement evenement, int persoonId);
    ErrorCodeEnum DeleteDeelnemer(Evenement evenement, Persoon persoon);
    ErrorCodeEnum DeleteDeelnemer(Evenement evenement, int persoonId);
}
