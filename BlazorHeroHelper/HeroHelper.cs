using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlazorHeroHelper
{
    public partial class HeroHelper : Form
    {
        public HeroHelper()
        {
            InitializeComponent();
        }

        private string GetNamespaceFromProject(string projectFile)
        {
            var fileContent = System.IO.File.ReadAllText(projectFile);
            var nameSpace = fileContent.Substring(fileContent.IndexOf("<RootNamespace>") + "<RootNamespace>".Length, fileContent.IndexOf("</RootNamespace>") - fileContent.IndexOf("<RootNamespace>") - "<RootNamespace>".Length);
            return nameSpace;
        }

        private void WriteConfig(string path, string content)
        {
            try { 
                new FileInfo(path).Directory.Create();
            } catch (Exception) { }
            if (File.Exists(path))
            {
                MessageBox.Show("File " + path + " is already existed... Aborting...");
                return;
            }
            File.WriteAllText(path, content);
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            using(var ofd = new OpenFileDialog())
            {
                ofd.ShowDialog();
                if (ofd.FileName != null)
                {
                    var dirName = System.IO.Path.GetDirectoryName(ofd.FileName) + "\\src";
                    appDir.Text = dirName + "\\Application";
                    clientDir.Text = dirName + "\\Client";
                    clientInfraDir.Text = dirName + "\\Client.Infrastructure";
                    domainDir.Text = dirName + "\\Domain";
                    infraDir.Text = dirName + "\\Infrastructure";
                    infraSharedDir.Text = dirName + "\\Infrastructure.Shared";
                    serverDir.Text = dirName + "\\Server";
                    sharedDir.Text = dirName + "\\Shared";
                    textBox1.Text = dirName;
                }
            }
        }

        private string CreateEntity()
        {
            var domainProject = domainDir.Text + "\\Domain.csproj";
            var nameSpace = GetNamespaceFromProject(domainProject);
            var entityPath = domainDir.Text + "\\Entities\\";
            if (folderPrefix.Text.Length != 0)
                entityPath += folderPrefix.Text + "\\";
            entityPath += entityName.Text + ".cs";
            var entityContent = Templates.Entity.TemplateCode.Replace("$_CONTRACTS_$", nameSpace + ".Contracts");
            entityContent = entityContent.Replace("$_ENUMS_$", nameSpace + ".Enums");

            var entityNamespace = "";
            if (folderPrefix.Text.Length != 0)
            {
                entityNamespace = nameSpace + ".Entities." + folderPrefix.Text;
                entityContent = entityContent.Replace("$_NAMESPACE_$", nameSpace + ".Entities." + folderPrefix.Text);
            }
            else {
                entityNamespace = nameSpace + ".Entities";
                entityContent = entityContent.Replace("$_NAMESPACE_$", nameSpace + ".Entities");
            }

            entityContent = entityContent.Replace("$_ENTITY_$", entityName.Text);
            entityContent = entityContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(entityPath, entityContent);

            return entityNamespace;
        }

        private string CreateIRepository()
        {
            var domainProject = domainDir.Text + "\\Domain.csproj";
            var domainnameSpace = GetNamespaceFromProject(domainProject);
            var domainContract = domainnameSpace + ".Contracts";
            var applicationProject = appDir.Text + "\\Application.csproj";
            var nameSpace = GetNamespaceFromProject(applicationProject);
            var iRepositoryPath = appDir.Text + "\\Interfaces\\Repositories\\";
            iRepositoryPath += "I" + entityName.Text + "Repository.cs";
            var iRepositoryContent = Templates.IRepository.TemplateCode.Replace("$_CONTRACTS_$", domainContract);
            iRepositoryContent = iRepositoryContent.Replace("$_NAMESPACE_$", nameSpace + ".Interfaces.Repositories");
            iRepositoryContent = iRepositoryContent.Replace("$_MODEL_$", entityName.Text);
            iRepositoryContent = iRepositoryContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(iRepositoryPath, iRepositoryContent);

            return nameSpace + ".Interfaces.Repositories";

        }

        private string CreateRepository(string nameSpaceInterface, string nameSpaceEntity)
        {
            var infraProject = infraDir.Text + "\\Infrastructure.csproj";
            var infraNameSpace = GetNamespaceFromProject(infraProject);
            var repositoryNamespace = infraNameSpace + ".Repositories";

            var repositoryPath = infraDir.Text + "\\Repositories\\";
            repositoryPath += entityName.Text + "Repository.cs";

            var repositoryContent = Templates.Repository.TemplateCode.Replace("$_INTERFACE_REPOSITORY_NAMESPACE_$", nameSpaceInterface);
            repositoryContent = repositoryContent.Replace("$_ENTITY_NAMESPACE_$", nameSpaceEntity);
            repositoryContent = repositoryContent.Replace("$_NAMESPACE_$", repositoryNamespace);
            repositoryContent = repositoryContent.Replace("$_ENTITY_$", entityName.Text);
            repositoryContent = repositoryContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(repositoryPath, repositoryContent);

            return repositoryNamespace;
        }

        private string CreateAddEditCQRS(string iRepositoryNamespace, string entityNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);
            var addEditCQRSNamespace = appNameSpace + ".Features.";
            if (folderPrefix.Text.Length != 0)
            {
                addEditCQRSNamespace += folderPrefix.Text + ".";
            }
            addEditCQRSNamespace += entityName.Text + "s.Commands.AddEdit";


            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var addEditCQRSPath = appDir.Text + "\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                addEditCQRSPath += folderPrefix.Text + "\\";
            }

            addEditCQRSPath += entityName.Text + "s\\Commands\\AddEdit\\";
            addEditCQRSPath += "AddEdit" + entityName.Text + "Command.cs";

            var addEditCQRSContent = Templates.AddEditCommand.TemplateCode.Replace("$_INTERFACE_REPOSITORY_NAMESPACE_$", iRepositoryNamespace);
            addEditCQRSContent = addEditCQRSContent.Replace("$_SHARED_WRAPPER_NAMESPACE_$", sharedWrapperNamespace);
            addEditCQRSContent = addEditCQRSContent.Replace("$_NAMESPACE_$", addEditCQRSNamespace);
            addEditCQRSContent = addEditCQRSContent.Replace("$_ENTITY_NAMESPACE_$", entityNamespace);
            addEditCQRSContent = addEditCQRSContent.Replace("$_ENTITY_$", entityName.Text);
            addEditCQRSContent = addEditCQRSContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(addEditCQRSPath, addEditCQRSContent);

            return addEditCQRSNamespace;
        }

        private string CreateDeleteCQRS(string iRepositoryNamespace, string entityNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);
            var deleteCQRSNamespace = appNameSpace + ".Features.";
            if (folderPrefix.Text.Length != 0)
            {
                deleteCQRSNamespace += folderPrefix.Text + ".";
            }
            deleteCQRSNamespace += entityName.Text + "s.Commands.Delete";


            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var deleteCQRSPath = appDir.Text + "\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                deleteCQRSPath += folderPrefix.Text + "\\";
            }

            deleteCQRSPath += entityName.Text + "s\\Commands\\Delete\\";
            deleteCQRSPath += "Delete" + entityName.Text + "Command.cs";

            var deleteCQRSContent = Templates.DeleteCommand.TemplateCode.Replace("$_INTERFACE_REPOSITORY_NAMESPACE_$", iRepositoryNamespace);
            deleteCQRSContent = deleteCQRSContent.Replace("$_SHARED_WRAPPER_NAMESPACE_$", sharedWrapperNamespace);
            deleteCQRSContent = deleteCQRSContent.Replace("$_NAMESPACE_$", deleteCQRSNamespace);
            deleteCQRSContent = deleteCQRSContent.Replace("$_ENTITY_NAMESPACE_$", entityNamespace);
            deleteCQRSContent = deleteCQRSContent.Replace("$_ENTITY_$", entityName.Text);
            deleteCQRSContent = deleteCQRSContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(deleteCQRSPath, deleteCQRSContent);

            return deleteCQRSNamespace;
        }

        private string CreateSpecification(string entityNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);
            var specificationNamespace = appNameSpace + ".Specifications";
            if (folderPrefix.Text.Length != 0)
            {
                specificationNamespace += "." + folderPrefix.Text;
            }

            var specificationBaseNamespace = appNameSpace + ".Specifications.Base";

            var specificationPath = appDir.Text + "\\Specifications\\";
            if (folderPrefix.Text.Length != 0)
            {
                specificationPath += folderPrefix.Text + "\\";
            }

            specificationPath += entityName.Text + "FilterSpecification.cs";

            var specificationContent = Templates.FilterSpecification.TemplateCode.Replace("$_SPECIFICATION_BASE_NAMESPACE_$", specificationBaseNamespace);
            specificationContent = specificationContent.Replace("$_ENTITY_NAMESPACE_$", entityNamespace);
            specificationContent = specificationContent.Replace("$_NAMESPACE_$", specificationNamespace);
            specificationContent = specificationContent.Replace("$_ENTITY_$", entityName.Text);
            specificationContent = specificationContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(specificationPath, specificationContent);

            return specificationNamespace;
        }

        private string CreateMappingProfile(string entityNamespace, string addEditCQRSNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);
            var mappingProfileNamespace = appNameSpace + ".Mappings";


            var mappingProfilePath = appDir.Text + "\\Mappings\\";

            mappingProfilePath += entityName.Text + "Profile.cs";

            var mappingProfileContent = Templates.MappingProfile.TemplateCode.Replace("$_ADD_EDIT_CQRS_NAMESPACE_$", addEditCQRSNamespace);
            mappingProfileContent = mappingProfileContent.Replace("$_ENTITY_NAMESPACE_$", entityNamespace);
            mappingProfileContent = mappingProfileContent.Replace("$_NAMESPACE_$", mappingProfileNamespace);
            mappingProfileContent = mappingProfileContent.Replace("$_ENTITY_$", entityName.Text);
            mappingProfileContent = mappingProfileContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);


            WriteConfig(mappingProfilePath, mappingProfileContent);

            return mappingProfileNamespace;
        }

        private string CreateGetAllPagedQuery(string iRepositoryNamespace, string entityNamespace, string filterSpecNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);
            var extensionNamespace = appNameSpace + ".Extensions";
            var getAllPagedQueryNamespace = appNameSpace + ".Features.";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedQueryNamespace += folderPrefix.Text + ".";
            }
            getAllPagedQueryNamespace += entityName.Text + "s.Queries.GetAllPaged";


            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var getAllPagedQueryPath = appDir.Text + "\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedQueryPath += folderPrefix.Text + "\\";
            }

            getAllPagedQueryPath += entityName.Text + "s\\Queries\\GetAllPaged\\";
            getAllPagedQueryPath += "GetAll" + entityName.Text + "sQuery.cs";

            var getAllPagedQueryContent = Templates.GetAllPagedQuery.TemplateCode.Replace("$_EXTENSION_NAMESPACE_$", extensionNamespace);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_INTERFACE_REPOSITORY_NAMESPACE_$", iRepositoryNamespace);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_FILTER_SPECIFICATION_NAMESPACE_$", filterSpecNamespace);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_SHARED_WRAPPER_NAMESPACE_$", sharedWrapperNamespace);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_NAMESPACE_$", getAllPagedQueryNamespace);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_ENTITY_NAMESPACE_$", entityNamespace);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_ENTITY_$", entityName.Text);
            getAllPagedQueryContent = getAllPagedQueryContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(getAllPagedQueryPath, getAllPagedQueryContent);

            return getAllPagedQueryNamespace;
        }

        private string CreateGetAllPagedResponse(string iRepositoryNamespace, string entityNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);

            var domainProject = domainDir.Text + "\\Domain.csproj";
            var domainNamespace = GetNamespaceFromProject(appProject);
            var enumNamespace = domainNamespace + ".Enums";

            var extensionNamespace = appNameSpace + ".Extensions";
            var getAllPagedResponseNamespace = appNameSpace + ".Features.";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedResponseNamespace += folderPrefix.Text + ".";
            }
            getAllPagedResponseNamespace += entityName.Text + "s.Queries.GetAllPaged";


            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var getAllPagedResponsePath = appDir.Text + "\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedResponsePath += folderPrefix.Text + "\\";
            }

            getAllPagedResponsePath += entityName.Text + "s\\Queries\\GetAllPaged\\";
            getAllPagedResponsePath += "GetAllPaged" + entityName.Text + "sResponse.cs";

            var getAllPagedResponseContent = Templates.GetAllPagedResponse.TemplateCode.Replace("$_ENUM_NAMESPACE_$", enumNamespace);
            getAllPagedResponseContent = getAllPagedResponseContent.Replace("$_NAMESPACE_$", getAllPagedResponseNamespace);
            getAllPagedResponseContent = getAllPagedResponseContent.Replace("$_ENTITY_$", entityName.Text);
            getAllPagedResponseContent = getAllPagedResponseContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(getAllPagedResponsePath, getAllPagedResponseContent);

            return getAllPagedResponseNamespace;
        }

        private string CreateGetModelByIdQuery(string iRepositoryNamespace, string entityNamespace, string filterSpecNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);
            var extensionNamespace = appNameSpace + ".Extensions";
            var getAllPagedQueryNamespace = appNameSpace + ".Features.";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedQueryNamespace += folderPrefix.Text + ".";
            }
            getAllPagedQueryNamespace += entityName.Text + "s.Queries.GetById";


            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var getModelByIdQueryPath = appDir.Text + "\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                getModelByIdQueryPath += folderPrefix.Text + "\\";
            }

            getModelByIdQueryPath += entityName.Text + "s\\Queries\\GetById\\";
            getModelByIdQueryPath += "Get" + entityName.Text + "ByIdQuery.cs";

            var getModelByIdQueryContent = Templates.GetModelByIdQuery.TemplateCode.Replace("$_EXTENSION_NAMESPACE_$", extensionNamespace);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_INTERFACE_REPOSITORY_NAMESPACE_$", iRepositoryNamespace);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_FILTER_SPECIFICATION_NAMESPACE_$", filterSpecNamespace);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_SHARED_WRAPPER_NAMESPACE_$", sharedWrapperNamespace);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_NAMESPACE_$", getAllPagedQueryNamespace);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_ENTITY_NAMESPACE_$", entityNamespace);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_ENTITY_$", entityName.Text);
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_ENTITY_LOWER_$", entityName.Text.ToLower());
            getModelByIdQueryContent = getModelByIdQueryContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(getModelByIdQueryPath, getModelByIdQueryContent);

            return getAllPagedQueryNamespace;
        }

        private string CreateGetModelByIdResponse(string iRepositoryNamespace, string entityNamespace)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);

            var domainProject = domainDir.Text + "\\Domain.csproj";
            var domainNamespace = GetNamespaceFromProject(appProject);
            var enumNamespace = domainNamespace + ".Enums";

            var extensionNamespace = appNameSpace + ".Extensions";
            var getAllPagedResponseNamespace = appNameSpace + ".Features.";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedResponseNamespace += folderPrefix.Text + ".";
            }
            getAllPagedResponseNamespace += entityName.Text + "s.Queries.GetById";


            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var getAllPagedResponsePath = appDir.Text + "\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                getAllPagedResponsePath += folderPrefix.Text + "\\";
            }

            getAllPagedResponsePath += entityName.Text + "s\\Queries\\GetById\\";
            getAllPagedResponsePath += "Get" + entityName.Text + "ByIdResponse.cs";

            var getModelByIdResponseContent = Templates.GetModelByIdResponse.TemplateCode.Replace("$_ENUM_NAMESPACE_$", enumNamespace);
            getModelByIdResponseContent = getModelByIdResponseContent.Replace("$_NAMESPACE_$", getAllPagedResponseNamespace);
            getModelByIdResponseContent = getModelByIdResponseContent.Replace("$_ENTITY_$", entityName.Text);
            getModelByIdResponseContent = getModelByIdResponseContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(getAllPagedResponsePath, getModelByIdResponseContent);

            return getAllPagedResponseNamespace;
        }
        private string CreateController(string addEditCQRS, string deleteCQRS, string getAllPagedCQRS, string getModelByIdCQRS)
        {
            var serverProject = serverDir.Text + "\\Server.csproj";
            var serverNamespace = GetNamespaceFromProject(serverProject);
            var serverControllersNamespace = serverNamespace + ".Controllers";

            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNamespace = GetNamespaceFromProject(sharedProject);

            var constPermissionNamespace = sharedNamespace + ".Constants.Permission";

            var controllerNamespace = serverNamespace + ".Controllers.v1";

            if (folderPrefix.Text.Length != 0)
            {
                controllerNamespace += "." + folderPrefix.Text;
            }

            var controllerPath = serverDir.Text + "\\Controllers\\v1\\";
            if (folderPrefix.Text.Length != 0)
            {
                controllerPath += folderPrefix.Text + "\\";
            }

            controllerPath += entityName.Text + "sController.cs";

            var controllerContent = Templates.Controller.TemplateCode.Replace("$_ADD_EDIT_COMMAND_NAMESPACE_$", addEditCQRS);
            controllerContent = controllerContent.Replace("$_DELETE_COMMAND_NAMESPACE_$", deleteCQRS);
            controllerContent = controllerContent.Replace("$_GET_ALL_PAGED_NAMESPACE_$", getAllPagedCQRS);
            controllerContent = controllerContent.Replace("$_GET_MODEL_BY_ID_CQRS_NAMESPACE_$", getModelByIdCQRS);
            controllerContent = controllerContent.Replace("$_SHARED_CONST_PERMISSION_NAMESPACE_$", constPermissionNamespace);
            controllerContent = controllerContent.Replace("$_NAMESPACE_$", controllerNamespace);
            controllerContent = controllerContent.Replace("$_ENTITY_$", entityName.Text);
            controllerContent = controllerContent.Replace("$_SERVER_CONTROLLER_NAMESPACE_$", serverControllersNamespace);
            controllerContent = controllerContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(controllerPath, controllerContent);

            controllerHelper.Text = controllerContent;

            return controllerNamespace;
        }

        private string CreateRoute()
        {
            var clientInfraProject = clientInfraDir.Text + "\\Client.Infrastructure.csproj";
            var clientInfraNamespace = GetNamespaceFromProject(clientInfraProject);

            var routeNamespace = clientInfraNamespace + ".Routes";
            var routePath = clientInfraDir.Text + "\\Routes\\" + entityName.Text + "sEndpoints.cs";
            var routeContent = Templates.Route.TemplateCode.Replace("$_NAMESPACE_$", routeNamespace);
            routeContent = routeContent.Replace("$_ENTITY_$", entityName.Text);
            routeContent = routeContent.Replace("$_ENTITY_LOWER_$", entityName.Text.ToLower());
            routeContent = routeContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(routePath, routeContent);

            routeHelper.Text = routeContent;

            return routeNamespace;
        }

        private string CreateRequest()
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);

            var requestNamespace = appNameSpace + ".Requests";
            if (folderPrefix.Text.Length != 0)
            {
                requestNamespace += "." + folderPrefix.Text;
            }

            var requestPath = appDir.Text + "\\Requests\\";
            if (folderPrefix.Text.Length != 0)
            {
                requestPath += folderPrefix.Text + "\\";
            }

            requestPath += "GetAllPaged" + entityName.Text + "sRequest.cs";

            var requestContent = Templates.Request.TemplateCode.Replace("$_NAMESPACE_$", requestNamespace);
            requestContent = requestContent.Replace("$_ENTITY_$", entityName.Text);
            requestContent = requestContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(requestPath, requestContent);

            

            return requestNamespace;
        }

        private string CreateManager(string addEditCQRS, string getAllPagedCQRS, string request, string getModelByIdCQRS)
        {
            var sharedProject = sharedDir.Text + "\\Shared.csproj";
            var sharedNameSpace = GetNamespaceFromProject(sharedProject);

            var sharedWrapperNamespace = sharedNameSpace + ".Wrapper";

            var clientInfraProject = clientInfraDir.Text + "\\Client.Infrastructure.csproj";
            var clientInfraNamespace = GetNamespaceFromProject(clientInfraProject);
            var clientInfraExtensionNamespace = clientInfraNamespace + ".Extensions";

            var managerNamespace = clientInfraNamespace + ".Managers";

            if (folderPrefix.Text.Length != 0)
            {
                managerNamespace += "." + folderPrefix.Text;
            }

            managerNamespace += "." + entityName.Text;

            var managerPath = clientInfraDir.Text + "\\Managers\\";
            if (folderPrefix.Text.Length != 0)
            {
                managerPath += folderPrefix.Text + "\\";
            }

            managerPath += entityName.Text + "\\";
            var iManagerPath = managerPath + "I" + entityName.Text + "Manager.cs";
            managerPath += entityName.Text + "Manager.cs";

            var iManagerContent = Templates.IEntityManager.TemplateCode1.Replace("$_ADD_EDIT_CQRS_NAMESPACE_$", addEditCQRS);
            iManagerContent = iManagerContent.Replace("$_GET_ALL_PAGED_CQRS_NAMESPACE_$", getAllPagedCQRS);
            iManagerContent = iManagerContent.Replace("$_GET_MODEL_BY_ID_CQRS_NAMESPACE_$", getModelByIdCQRS);
            iManagerContent = iManagerContent.Replace("$_REQUEST_NAMESPACE_$", request);
            iManagerContent = iManagerContent.Replace("$_SHARED_WRAPPER_NAMESPACE_$", sharedWrapperNamespace);
            iManagerContent = iManagerContent.Replace("$_NAMESPACE_$", managerNamespace);
            iManagerContent = iManagerContent.Replace("$_ENTITY_$", entityName.Text);
            iManagerContent = iManagerContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            var managerContent = Templates.IEntityManager.TemplateCode2.Replace("$_ADD_EDIT_CQRS_NAMESPACE_$", addEditCQRS);
            managerContent = managerContent.Replace("$_GET_ALL_PAGED_CQRS_NAMESPACE_$", getAllPagedCQRS);
            managerContent = managerContent.Replace("$_GET_MODEL_BY_ID_CQRS_NAMESPACE_$", getModelByIdCQRS);
            managerContent = managerContent.Replace("$_REQUEST_NAMESPACE_$", request);
            managerContent = managerContent.Replace("$_SHARED_WRAPPER_NAMESPACE_$", sharedWrapperNamespace);
            managerContent = managerContent.Replace("$_CLIENT_INFRA_EXTENSION_NAMESPACE_$", clientInfraExtensionNamespace);
            managerContent = managerContent.Replace("$_NAMESPACE_$", managerNamespace);
            managerContent = managerContent.Replace("$_ENTITY_$", entityName.Text);
            managerContent = managerContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(iManagerPath, iManagerContent);
            WriteConfig(managerPath, managerContent);

            iManagerHelper.Text = iManagerContent;
            managerHelper.Text = managerContent;

            return managerNamespace;
        }

        private string CreateUI(string addEditCQRS, string getAllPagedCQRS, string request, string manager)
        {
            var clientProject = clientDir.Text + "\\Client.csproj";
            var sharedProject = sharedDir.Text + "\\Shared.csproj";

            var clientNamespace = GetNamespaceFromProject(clientProject);
            var sharedNamespace = GetNamespaceFromProject(sharedProject);

            var clientExtensionNamespace = clientNamespace + ".Extensions";
            var sharedConstantsApplicationNamespace = sharedNamespace + ".Constants.Application";
            var sharedConstantsPermissionNamespace = sharedNamespace + ".Constants.Permission";

            var lowerEntityName = entityName.Text.ToLower();

            var uiNamespace = clientNamespace + ".Pages";
            if (folderPrefix.Text.Length != 0)
            {
                uiNamespace += "." + folderPrefix.Text;
            }

            var uiFolder = clientDir.Text + "\\Pages\\";
            if (folderPrefix.Text.Length != 0)
            {
                uiFolder += folderPrefix.Text + "\\";
            }

            var uiListRazor = uiFolder + entityName.Text + "s.razor";
            var uiListCs = uiFolder + entityName.Text + "s.razor.cs";

            var uiModalRazor = uiFolder + "AddEdit" + entityName.Text + "Modal.razor";
            var uiModalCs = uiFolder + "AddEdit" + entityName.Text + "Modal.razor.cs";

            var pagePath = "/";
            if (folderPrefix.Text.Length != 0)
            {
                pagePath += folderPrefix.Text.ToLower() + "/";
            }
            pagePath += lowerEntityName + "s";

            var uiListRazorContent = Templates.UITemplate.ListRazor.Replace("$_PAGE_PATH_$", pagePath);
            uiListRazorContent = uiListRazorContent.Replace("$_ENTITY_$", entityName.Text);
            uiListRazorContent = uiListRazorContent.Replace("$_GET_ALL_PAGED_NAMESPACE_$", getAllPagedCQRS);
            uiListRazorContent = uiListRazorContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            var uiListCsContent = Templates.UITemplate.ListCS.Replace("$_GET_ALL_PAGED_CQRS_NAMESPACE_$", getAllPagedCQRS);
            uiListCsContent = uiListCsContent.Replace("$_REQUEST_NAMESPACE_$", request);
            uiListCsContent = uiListCsContent.Replace("$_CLIENT_EXTENSIONS_NAMESPACE_$", clientExtensionNamespace);
            uiListCsContent = uiListCsContent.Replace("$_SHARED_CONSTANTS_APPLICATION_NAMESPACE_$", sharedConstantsApplicationNamespace);
            uiListCsContent = uiListCsContent.Replace("$_ADD_EDIT_CQRS_NAMESPACE_$", addEditCQRS);
            uiListCsContent = uiListCsContent.Replace("$_MANAGER_NAMESPACE_$", manager);
            uiListCsContent = uiListCsContent.Replace("$_SHARED_CONSTANTS_PERMISSION_NAMESPACE_$", sharedConstantsPermissionNamespace);
            uiListCsContent = uiListCsContent.Replace("$_NAMESPACE_$", uiNamespace);
            uiListCsContent = uiListCsContent.Replace("$_ENTITY_$", entityName.Text);
            uiListCsContent = uiListCsContent.Replace("$_ENTITY_LOWER_$", lowerEntityName);
            uiListCsContent = uiListCsContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            var uiModalRazorContent = Templates.UITemplate.AddEditModalRazor.Replace("$_PAGE_PATH_$", pagePath);
            uiModalRazorContent = uiModalRazorContent.Replace("$_ENTITY_$", entityName.Text);
            uiModalRazorContent = uiModalRazorContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            var uiModalCsContent = Templates.UITemplate.AddEditModalCS.Replace("$_GET_ALL_PAGED_CQRS_NAMESPACE_$", getAllPagedCQRS);
            uiModalCsContent = uiModalCsContent.Replace("$_REQUEST_NAMESPACE_$", request);
            uiModalCsContent = uiModalCsContent.Replace("$_CLIENT_EXTENSIONS_NAMESPACE_$", clientExtensionNamespace);
            uiModalCsContent = uiModalCsContent.Replace("$_SHARED_CONSTANTS_APPLICATION_NAMESPACE_$", sharedConstantsApplicationNamespace);
            uiModalCsContent = uiModalCsContent.Replace("$_ADD_EDIT_CQRS_NAMESPACE_$", addEditCQRS);
            uiModalCsContent = uiModalCsContent.Replace("$_MANAGER_NAMESPACE_$", manager);
            uiModalCsContent = uiModalCsContent.Replace("$_SHARED_CONSTANTS_PERMISSION_NAMESPACE_$", sharedConstantsPermissionNamespace);
            uiModalCsContent = uiModalCsContent.Replace("$_NAMESPACE_$", uiNamespace);
            uiModalCsContent = uiModalCsContent.Replace("$_ENTITY_$", entityName.Text);
            uiModalCsContent = uiModalCsContent.Replace("$_ENTITY_LOWER_$", lowerEntityName);
            uiModalCsContent = uiModalCsContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(uiListRazor, uiListRazorContent);
            WriteConfig(uiListCs, uiListCsContent);
            WriteConfig(uiModalRazor, uiModalRazorContent);
            WriteConfig(uiModalCs, uiModalCsContent);

            uiListRazorHelper.Text = uiListRazorContent;
            uiListCsHelper.Text = uiListCsContent;
            uiModalRazorHelper.Text = uiModalRazorContent;
            uiModalCsHelper.Text = uiModalCsContent;

            return uiNamespace;
        }

        private string CreateValidator(string addEditCQRS)
        {
            var appProject = appDir.Text + "\\Application.csproj";
            var appNameSpace = GetNamespaceFromProject(appProject);

            var validatorNamespace = appNameSpace + ".Validators.Features";
            if (folderPrefix.Text.Length != 0)
            {
                validatorNamespace += "." + folderPrefix.Text;
            }

            validatorNamespace += "." + entityName.Text + ".Commands.AddEdit";

            var validatorPath = appDir.Text + "\\Validators\\Features\\";
            if (folderPrefix.Text.Length != 0)
            {
                validatorPath += folderPrefix.Text + "\\";
            }

            validatorPath += entityName.Text + "s\\Commands\\AddEdit\\AddEdit" + entityName.Text + "CommandValidator.cs";

            var validatorContent = Templates.Validation.TemplateCode.Replace("$_NAMESPACE_$", validatorNamespace);
            validatorContent = validatorContent.Replace("$_ENTITY_$", entityName.Text);
            validatorContent = validatorContent.Replace("$_ADD_EDIT_CQRS_NAMESPACE_$", addEditCQRS);
            validatorContent = validatorContent.Replace("$_DEFAULT_ID_DATATYPE_$", defaultDatatype.Text);

            WriteConfig(validatorPath, validatorContent);

            return validatorNamespace;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var entityNameSpace = CreateEntity();
            var iRepositoryNameSpace = CreateIRepository();
            var repositoryNameSpace = CreateRepository(iRepositoryNameSpace, entityNameSpace);
            var addEditCQRSNamespace = CreateAddEditCQRS(iRepositoryNameSpace, entityNameSpace);
            var deleteCQRSNamespace = CreateDeleteCQRS(iRepositoryNameSpace, entityNameSpace);
            var filterSpecNamespace = CreateSpecification(entityNameSpace);
            var mappingProfileNamespace = CreateMappingProfile(entityNameSpace, addEditCQRSNamespace);
            var getAllPagedQueryNamespace = CreateGetAllPagedQuery(iRepositoryNameSpace, entityNameSpace, filterSpecNamespace);
            var getAllPagedResponseNamespace = CreateGetAllPagedResponse(iRepositoryNameSpace, entityNameSpace);
            var getModelByIdQueryNamespace = CreateGetModelByIdQuery(iRepositoryNameSpace, entityNameSpace, filterSpecNamespace);
            var getModelByIdResponseNamespace = CreateGetModelByIdResponse(iRepositoryNameSpace, entityNameSpace);
            var controllerNamespace = CreateController(addEditCQRSNamespace, deleteCQRSNamespace, getAllPagedQueryNamespace, getModelByIdQueryNamespace);
            var routeNamespace = CreateRoute();
            var requestNamespace = CreateRequest();
            var managerNamespace = CreateManager(addEditCQRSNamespace, getAllPagedQueryNamespace, requestNamespace, getModelByIdQueryNamespace);
            var uiNamespace = CreateUI(addEditCQRSNamespace, getAllPagedQueryNamespace, requestNamespace, managerNamespace);
            var validatorNamespace = CreateValidator(addEditCQRSNamespace);

            dbContextHelper.Text = String.Format("public DbSet<{0}> {0}s {{ get; set; }}", entityName.Text);
            serviceRepositoryHelper.Text = String.Format(".AddTransient<I{0}Repository, {0}Repository>()", entityName.Text);
            permissionHelper.Text = Templates.Permission.TemplateCode.Replace("$_ENTITY_$", entityName.Text);
            MessageBox.Show("Done!");
        }

        private void HeroHelper_Load(object sender, EventArgs e)
        {
            defaultDatatype.SelectedIndex = 0;
        }
    }
}