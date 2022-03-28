Ext.define('Gnt.examples.advanced.store.Tasks', {
    extend                           : 'Gnt.data.TaskStore',

    requires                         : ['Gnt.examples.advanced.model.Project'],
    reApplyFilterOnDataChange        : false,
    alias                            : 'store.advanced-task-store',
    model                            : 'Gnt.examples.advanced.model.Task',
    filterer                         : 'bottomup',

    // If you experience performance issue you can turn normalization off,
    // but make sure you feed the store with already normalized data
    // autoNormalizeNodes               : false,

    // Schedule by constraints
    scheduleByConstraints            : true,
    // Activate UI to warn on:
    // - violating dependencies
    // - potential scheduling conflicts
    checkDependencyConstraint        : true,
    checkPotentialConflictConstraint : true
});
