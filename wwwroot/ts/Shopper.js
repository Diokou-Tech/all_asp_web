var Shopper = /** @class */ (function () {
    function Shopper(nom, prenom) {
        this.nom = nom;
        this.prenom = prenom;
    }
    Shopper.prototype.showName = function () {
        alert("my name is ${this.nom} and lastname is ${this.prenom}");
    };
    Shopper.prototype.setNom = function (name) {
        this.nom = name;
    };
    return Shopper;
}());
export { Shopper };
//# sourceMappingURL=Shopper.js.map