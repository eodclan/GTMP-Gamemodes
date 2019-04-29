var app = {
  valide: true,
  init: function() {
    document.getElementById('BtnConnect').addEventListener('click', app.connect);
    document.getElementById('BtnInscrip').addEventListener('click', app.pageInscription);
  },

  connect: function(evt) {
    evt.preventDefault();
    if (document.getElementById('PseudoConnexion').value.length !== 0 && document.getElementById('MDPConnexion').value.length !== 0) {
        resourceCall("sendInput", document.getElementById('PseudoConnexion').value, document.getElementById('MDPConnexion').value);
    }
  },

  inscription: function(evt) {
    evt.preventDefault();
    var identifiants = [
      document.getElementById('PseudoInscription').value,
      document.getElementById('EmailInscription').value,
      document.getElementById('MDPInscription').value,
      document.getElementById('ConfirmMDPInscription').value
    ];
    app.valide = true;

    if(document.getElementById('PseudoInscription').value.length === 0){
      document.getElementById('PseudoInscription').className += ' bg-danger border-danger text-white';
      document.getElementById('faPseudoInscription').className += ' text-danger';
      app.valide = false;
    }
    if (document.getElementById('NomIGInscription').value.length === 0 || document.getElementById('NomIGInscription').value.indexOf('_') === -1) {
        document.getElementById('NomIGInscription').className += ' bg-danger border-danger text-white';
        document.getElementById('faNomIGInscription').className += ' text-danger';
        app.valide = false;
    }
    if(document.getElementById('EmailInscription').value.length === 0 || !validEmail(document.getElementById('EmailInscription').value)){
      document.getElementById('EmailInscription').className += ' bg-danger border-danger text-white';
      document.getElementById('faEmailInscription').className += ' text-danger';
      app.valide = false;
    }
    if(document.getElementById('MDPInscription').value.length === 0){
      document.getElementById('MDPInscription').className += ' bg-danger border-danger text-white';
      document.getElementById('faMDPInscription').className += ' text-danger';
      app.valide = false;
    }
    if (document.getElementById('ConfirmMDPInscription').value.length === 0 || document.getElementById('ConfirmMDPInscription').value !== document.getElementById('MDPInscription').value){
      document.getElementById('ConfirmMDPInscription').className += ' bg-danger border-danger text-white';
      document.getElementById('faConfirmMDPInscription').className += ' text-danger';
      app.valide = false;
    }
    if (app.valide === true) {
        resourceCall("sendInputInscription", document.getElementById('PseudoInscription').value, document.getElementById('NomIGInscription').value, document.getElementById('EmailInscription').value, document.getElementById('MDPInscription').value);
      app.connexion(evt);
    }

    function validEmail(v) {
      var r = new RegExp("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
      return (v.match(r) === null) ? false : true;
    }


  },

  pageInscription: function(evt) {
    evt.preventDefault();
    document.getElementById('form').innerHTML = "<div class=\"form-group\"><label for=\"PseudoInscription\"><i id=\"faPseudoInscription\" class=\"fa fa-user\" aria-hidden=\"true\"></i></label><input type=\"text\" class=\"form-control\" id=\"PseudoInscription\" placeholder=\"Pseudo\"></div><div class=\"form-group\"><label for=\"NomIGInscription\"><i id=\"faNomIGInscription\" class=\"fa fa-user\" aria-hidden=\"true\"></i></label><input type=\"text\" class=\"form-control\" id=\"NomIGInscription\" placeholder=\"Prenom_Nom Personnage\"></div><div class=\"form-group\"><label for=\"EmailInscription\"><i id=\"faEmailInscription\" class=\"fa fa-user\" aria-hidden=\"true\"></i></i></label><input type=\"text\" class=\"form-control\" id=\"EmailInscription\" placeholder=\"Email\"></div><div class=\"form-group\"><label for=\"MDPInscription\"><i id=\"faMDPInscription\" class=\"fa fa-lock\" aria-hidden=\"true\"></i></label><input type=\"password\" class=\"form-control\" id=\"MDPInscription\" placeholder=\"Mot de passe\"></div><div class=\"form-group\"><label for=\"ConfirmMDPInscription\"><i id=\"faConfirmMDPInscription\" class=\"fa fa-lock\" aria-hidden=\"true\"></i></label><input type=\"password\" class=\"form-control\" id=\"ConfirmMDPInscription\" placeholder=\"Confirmation Mot de passe\"></div><div class=\"BtnsInscription\"><button id=\"BtnValiderInscri\" type=\"submit\" class=\"btn btn-primary\" style=\"background-color:#427dbf;cursor:pointer;\">Valider</button><button id=\"BtnAnnulerInscri\" type=\"submit\" class=\"btn btn-primary\" style=\"background-color:#427dbf;cursor:pointer;\">Annuler</button></div>";
    document.getElementById('BtnAnnulerInscri').addEventListener('click', app.connexion);
    document.getElementById('BtnValiderInscri').addEventListener('click', app.inscription);
    document.getElementById('BtnInscrip').style.display = 'none';
    document.getElementById('h2inscript').style.display = 'none';
  },

  connexion: function(evt) {
    evt.preventDefault();

    document.getElementById('form').innerHTML = "<div class=\"form-group\"><label for=\"PseudoConnexion\"><i class=\"fa fa-user\" aria-hidden=\"true\"></i></label><input type=\"text\" class=\"form-control\" id=\"PseudoConnexion\" placeholder=\"Pseudo\"></div><div class=\"form-group\"><label for=\"MDPConnexion\"><i class=\"fa fa-lock\" aria-hidden=\"true\"></i></label><input type=\"password\" class=\"form-control\" id=\"MDPConnexion\" placeholder=\"Mot de passe\"></div><button id=\"BtnConnect\" type=\"submit\" class=\"btn btn-primary\" style=\"background-color:#427dbf;cursor:pointer;\">Se connecter</button>";
    app.init();
    document.getElementById('BtnInscrip').style.display = 'block';
    document.getElementById('h2inscript').style.display = 'block';
  }
};

document.addEventListener('DOMContentLoaded', function() {
  app.init();
});
