Questa guida descrive i passi principali per pubblicare l'applicazione **CSharpSales** su AWS 
utilizzando servizi gratuiti o in fascia free tier. 
L'obiettivo è eseguire il backend in cloud e gettare le basi per una pipeline di distribuzione automatica.

## 1. Prerequisiti

1. **Account AWS**
   - Registrati su [https://aws.amazon.com/](https://aws.amazon.com/) e attiva il piano gratuito 
   - (richiede carta di credito ma non comporta costi se si rimane nei limiti del free tier).
2. **AWS CLI**
   - Installa l'interfaccia a riga di comando seguendo la guida ufficiale: 
     - [Installazione AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html).
   - Configura le credenziali con `aws configure` (Access key, Secret key, regione e formato output preferito).
3. **.NET SDK**
   - Già necessario per il progetto. Verifica che `dotnet --version` restituisca la versione installata.
4. **EB CLI (facoltativo ma consigliato)**
   - Elastic Beanstalk CLI facilita la creazione e il deploy dell'applicazione. Installazione: 
     - [EB CLI](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/eb-cli3-install.html).

## 2. Scelta del servizio AWS

Per un semplice backend ASP.NET Core la via più immediata è **Elastic Beanstalk** con runtime .NET su EC2.
Il free tier di AWS copre 750 ore al mese di un'istanza `t2.micro` per 12 mesi, sufficiente per ambienti di test o piccole demo.

Altre alternative (non trattate in dettaglio):
- **AWS App Runner**: più semplice se si dispone di un'immagine Docker. 
    Offre un periodo gratuito di 90 giorni.
- **AWS Lambda**: esecuzione serverless, possibile ma richiede adattamenti 
  (ad esempio l'uso di API Gateway e del runtime .NET per Lambda).

## 3. Pubblicazione con Elastic Beanstalk

1. **Preparare il progetto**
   ```bash
   dotnet publish -c Release -o publish
   ```
   Questo crea la cartella `publish` con i file necessari al runtime.
2. **Inizializzare Elastic Beanstalk**
   ```bash
   eb init
   ```
   - Scegli il nome dell'applicazione e la regione.
   - Seleziona il platform “.NET on Linux”.
3. **Creare un ambiente**
   ```bash
   eb create CSharpSales-env
   ```
   Questo comando avvia un'istanza EC2 sotto Elastic Beanstalk e imposta l'ambiente.
4. **Deploy dell'applicazione**
   ```bash
   eb deploy
   ```
   Al termine verrà fornito un URL pubblico 
   (ad esempio `http://sales-develop.us-east-1.elasticbeanstalk.com/`).
   L'API sarà raggiungibile all'endpoint `/GetCartResponse` di quell'URL.
1. 
5. **Gestire l'ambiente**
   - `eb open` apre il browser sull'URL appena creato.
   - `eb logs` permette di consultare i log in caso di problemi.

Ricorda che la fascia gratuita copre una sola istanza `t2.micro`. 
Controlla regolarmente la **console billing** di AWS per evitare costi imprevisti.

## 4. Spunti per una pipeline di deploy

Una volta funzionante il deploy manuale, si può automatizzare con GitHub Actions o AWS CodePipeline.

Ecco un possibile approccio con GitHub Actions:

1. Crea un utente IAM dedicato alle azioni con permessi per Elastic Beanstalk 
     (ad esempio policy `AWSElasticBeanstalkFullAccess`).
2. Aggiungi le sue chiavi di accesso come **Secret** nel repository GitHub 
     (`AWS_ACCESS_KEY_ID` e `AWS_SECRET_ACCESS_KEY`).
3. Aggiungi un workflow `.github/workflows/deploy.yml` che esegue:
   - `dotnet publish` del progetto
   - configurazione dell'AWS CLI con le credenziali
   - `eb deploy` verso l'ambiente desiderato

Esempio molto semplificato di workflow:
```yaml
name: Deploy to AWS EB
on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'
    - run: dotnet publish -c Release -o publish
    - name: Configure AWS credentials
      uses: aws-actions/configure-aws-credentials@v4
      with:
        aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
        aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
        aws-region: us-east-1
    - name: Deploy
      run: |
        pip install awsebcli
        eb init -p 'net-core' CSharpSales
        eb deploy CSharpSales-env
```

Questo file avvia il deploy automatico ogni volta che viene effettuato un push sul branch `main`.
Personalizza nome dell'ambiente e regione in base alla tua configurazione.

## 5. Risorse utili da approfondire

- [Guida ufficiale Elastic Beanstalk](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/create_deploy_NET.container.html)
- [AWS Free Tier](https://aws.amazon.com/free/)
- [GitHub Actions per AWS](https://github.com/aws-actions)

Con questi passaggi dovresti poter mettere online il backend e sperimentare una prima pipeline di pubblicazione.