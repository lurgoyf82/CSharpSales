# Deploying a .NET 8/9 Application to AWS Elastic Beanstalk

This guide shows how to provision Elastic Beanstalk (EB) and connect 
the repository’s **GitHub Actions** pipeline for continuous deployment.

---

## 1. Create the EB Application and Environment

1. **AWS Console ? Elastic Beanstalk ? Create application**  
2. *Application name*: `ProjectName`  
3. *Environment type*: **Web server**  
4. *Platform*: **.NET on Amazon Linux** ? Choose the latest branch that supports .NET 8 or .NET 9.  
5. *Application code*: leave **Sample application** (the pipeline will overwrite it).  
6. *Environment name*: `ProjectName-env` (must match the pipeline).  
7. Launch. Wait for **Health = Healthy** and note the environment URL.

---

## 2. IAM Credentials for GitHub Actions

1. **IAM ? Users ? Add user**  
   *User name*: `github-actions-deployer`  
   *Access type*: **Programmatic access**  
2. Attach policies:  
   - **AWSElasticBeanstalkFullAccess**  
   - **AmazonS3FullAccess**  
3. Finish and copy the **Access key ID** and **Secret access key**.

Add these secrets to the GitHub repo:

| Secret name              | Value (from IAM) |
|--------------------------|------------------|
| `AWS_ACCESS_KEY_ID`      | *Access key ID*  |
| `AWS_SECRET_ACCESS_KEY`  | *Secret key*     |

---

## 3. GitHub Actions Workflow (already in repo)

```yaml
on:
  push:
    branches: [ develop ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - run: |
          dotnet publish ./src/ProjectName/ProjectName.csproj -c Release -o publish
          cd publish
          zip -r deploy.zip .

      - uses: aws-actions/configure-aws-credentials@v4
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: us-east-1

      - uses: einaregilsson/beanstalk-deploy@v21
        with:
          application_name: ProjectName
          environment_name: ProjectName-env
          version_label: ${{ github.sha }}
          region: us-east-1
          deployment_package: publish/deploy.zip

```
