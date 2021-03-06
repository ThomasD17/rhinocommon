#pragma warning disable 1591
using System;

#if RHINO_SDK
namespace Rhino.DocObjects
{
  public class DetailViewObject : RhinoObject 
  {
    internal DetailViewObject(uint serialNumber)
      : base(serialNumber) { }

    public Rhino.Geometry.DetailView DetailGeometry
    {
      get
      {
        Rhino.Geometry.DetailView rc = Geometry as Rhino.Geometry.DetailView;
        return rc;
      }
    }

    internal override CommitGeometryChangesFunc GetCommitFunc()
    {
      return UnsafeNativeMethods.CRhinoDetailObject_InternalCommitChanges;
    }

    public bool IsActive
    {
      get
      {
        IntPtr pConstThis = ConstPointer();
        return UnsafeNativeMethods.CRhinoDetailViewObject_IsActive(pConstThis);
      }
      set
      {
        IntPtr pConstThis = ConstPointer();
        UnsafeNativeMethods.CRhinoDetailViewObject_SetActive(pConstThis, value);
      }
    }

    Rhino.Display.RhinoViewport m_viewport;
    public Rhino.Display.RhinoViewport Viewport
    {
      get { return m_viewport ?? (m_viewport = new Rhino.Display.RhinoViewport(this)); }
    }

    public bool CommitViewportChanges()
    {
      bool rc = false;
      if (m_viewport != null)
      {
        IntPtr pThis = ConstPointer();
        IntPtr pViewport = m_viewport.ConstPointer();
        uint serial_number = UnsafeNativeMethods.CRhinoDetailViewObject_CommitViewportChanges(pThis, pViewport);
        if (serial_number > 0)
        {
          rc = true;
          m_rhinoobject_serial_number = serial_number;
          m_viewport.OnDetailCommit();
        }
      }
      return rc;
    }
  }
}
#endif