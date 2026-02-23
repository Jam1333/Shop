pipeline {
    agent {
        node {
            label 'docker-agent-alpine'
        }
    }
    stages {
        stage('Build') {
            steps {
                // .slnx is supported in .NET 10 - use it directly
                sh 'dotnet restore Shop.slnx'
                sh 'dotnet build --no-restore --configuration Release Shop.slnx'
            }
        }
        stage('Test') {
            steps {
                sh 'dotnet test --no-build --configuration Release --verbosity normal Shop.slnx'
            }
        }
        stage('Deploy') {
            steps {
                echo "Deploying..."
            }
        }
    }
}
