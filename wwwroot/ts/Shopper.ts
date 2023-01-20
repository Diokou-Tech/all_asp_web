export class Shopper {
    constructor(private nom:string, private prenom: string)
    { }
    showName()
    {
        alert("my name is ${this.nom} and lastname is ${this.prenom}");
    }
    setNom(name: string) {
        this.nom = name;
    }
}