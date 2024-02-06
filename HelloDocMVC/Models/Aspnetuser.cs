using System;
using System.Collections;
using System.Collections.Generic;

namespace HelloDocMVC.Models;

public partial class Aspnetuser
{
    public string Aspnetuserid { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Passwordhash { get; set; }

    public string? Securitystamp { get; set; }

    public string? Email { get; set; }

    public BitArray Emailconfirmed { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public BitArray Phonenumberconfirmed { get; set; } = null!;

    public BitArray Twofactorenabled { get; set; } = null!;

    public DateTime? Lockoutenddateutc { get; set; }

    public BitArray Lockoutenabled { get; set; } = null!;

    public int Accessfailedcount { get; set; }

    public string? Ip { get; set; }

    public string? Corepasswordhash { get; set; }

    public int? Hashversion { get; set; }

    public DateTime? Modifieddate { get; set; }

    public virtual ICollection<Admin> AdminAspnetusers { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminCreatedbyNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminModifiedbyNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Aspnetrole> Roles { get; set; } = new List<Aspnetrole>();
}
