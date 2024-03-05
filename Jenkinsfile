pipeline {
    agent any
 
    stages {
        stage('checkout') {
            steps {
                git branch: 'Pipeline', credentialsId: 'githup', url: 'https://github.com/mina-sobhi/SpatiumCMS.git'   
                echo 'checkouting...'
            }
        }
        stage('Building') {
            steps {
                sh 'dotnet build'
                echo 'Building...'
            }
        }
        stage('Testing') {
            steps {
                sh 'dotnet test'
                echo 'testing...'
            }
        }
        stage('Restoring') {
            steps {
                sh 'dotnet restore'
                echo 'restoring...'
            }
        }
        stage('Publishing') {
            steps {
                sh 'dotnet publish -c Release -o ./publish'  
                echo 'publishing ...'
            }
        }
        stage("Build Docker Image"){
            steps{
                sh 'docker build -t abdelrahman9655/cms_backend  -f SpatiumHR/Dockerfile .'
            }
        }
        stage('Login To Dockerhub'){
            steps{
                withCredentials([usernamePassword(credentialsId:'dockerhub', usernameVariable:'USERNAME', passwordVariable: 'PASSWORD')]){
                sh'echo $PASSWORD | docker login -u $USERNAME --password-stdin'
                }
            }
        }
        stage("Push Docker Image"){
            steps{
                sh 'docker push abdelrahman9655/cms_backend:$BUILD_NUMBER'
            }
        }
        stage('Delete Local Image') {
            steps {
                sh 'docker rmi abdelrahman9655/cms_backend:$BUILD_NUMBER'
            }
        }
    }
    post{
        always{
            sh 'docker logout'
        }
    }
}
