﻿using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;



namespace ReactiveSignalR.Server.Hubs
{
	/// <summary>
	/// Provides the ActionFilterAttribute invoking module.
	/// </summary>
	public sealed class ActionFilterModule : HubPipelineModule
	{
		#region Constructors
		/// <summary>
		/// Creates instance.
		/// </summary>
		public ActionFilterModule()
		{}
		#endregion


		#region HubPipelineModule Overrides
		/// <summary>
		/// This method is called before the incoming components of any modules added later to the <see cref="IHubPipeline"/> are
		/// executed. If this returns false, then those later-added modules and the server-side hub method invocation will not
		/// be executed. Even if a client has not been authorized to connect to a hub, it will still be authorized to invoke
		/// server-side methods on that hub unless it is prevented in <see cref="IHubPipelineModule.BuildIncoming"/> by not
		/// executing the invoke parameter or prevented in <see cref="HubPipelineModule.OnBeforeIncoming"/> by returning false.
		/// </summary>
		/// <param name="context">A description of the server-side hub method invocation.</param>
		/// <returns>
		/// true, if the incoming components of later added modules and the server-side hub method invocation should be executed;
		/// false, otherwise.
		/// </returns>
		protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
		{
			var attributes = context.MethodDescriptor.Attributes.OfType<ActionFilterAttribute>();
			foreach (var attribute in attributes)
				if (!attribute.OnBeforeIncoming(context))
					return false;
			return base.OnBeforeIncoming(context);
		}


		/// <summary>
		/// This method is called after the incoming components of any modules added later to the <see cref="IHubPipeline"/>
		/// and the server-side hub method have completed execution.
		/// </summary>
		/// <param name="result">The return value of the server-side hub method</param>
		/// <param name="context">A description of the server-side hub method invocation.</param>
		/// <returns>The possibly new or updated return value of the server-side hub method</returns>
		protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
		{
			var attributes = context.MethodDescriptor.Attributes.OfType<ActionFilterAttribute>();
			foreach (var attribute in attributes)
				attribute.OnAfterIncoming(result, context);
			return base.OnAfterIncoming(result, context);
		}


		/// <summary>
		/// This is called when an uncaught exception is thrown by a server-side hub method or the incoming component of a
		/// module added later to the <see cref="IHubPipeline"/>. Observing the exception using this method will not prevent
		/// it from bubbling up to other modules.
		/// </summary>
		/// <param name="ex">The exception that was thrown during the server-side invocation.</param>
		/// <param name="context">A description of the server-side hub method invocation.</param>
		protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
		{
			var attributes = invokerContext.MethodDescriptor.Attributes.OfType<ActionFilterAttribute>();
			foreach (var attribute in attributes)
				attribute.OnIncomingError(exceptionContext, invokerContext);
			base.OnIncomingError(exceptionContext, invokerContext);
		}
		#endregion
	}
}